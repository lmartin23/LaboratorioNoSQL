using AutoMapper;
using FluentValidation.Results;
using LaboratorioNoSQL.Dtos;
using LaboratorioNoSQL.Enums;
using LaboratorioNoSQL.Models;
using LaboratorioNoSQL.Validators;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace LaboratorioNoSQL.Services
{
    public class UsuarioService
    {
        private IMongoCollection<Usuario> _usuarios;
        private readonly IMapper _mapper;
        public UsuarioService(IConexion conexion, IMapper mapper) //Obtengo el inject que hice en el startup
        {
            var client = new MongoClient(conexion.Server); //Crea la conexion 
            var database = client.GetDatabase(conexion.Database); // obtiene la base de datos
            _usuarios = database.GetCollection<Usuario>(conexion.Collection); // instancia la coleccion 
            _mapper = mapper;
        }

        public List<Usuario> Get()
        {
            return _usuarios.Find(d => true).ToList();
        }

        public Response<string> Post(BaseUserDto usu)
        {
            Response<string> res = new();
            res.Messagge = new List<string>();
            StatusCodeEnum statuscod = new();
            var validador = new BaseUserDtoValidator();
            ValidationResult result = validador.Validate(usu);
           if (usu.Email != "")
            {
                var user = _usuarios.Find<Usuario>(u => u.Email == usu.Email).FirstOrDefault();
                if (user != null)
                {
                    statuscod = StatusCodeEnum.EmailError;
                    res.CodStatus = (int)statuscod;
                    res.Messagge.Add(statuscod + " - Ya existe un usuario asociado a el email ingresado");
                    res.Obj = "";
                    res.Success = false;
                    return res;
                }
            }
            if (!result.IsValid )
            {
                statuscod = StatusCodeEnum.BadRequest;
                res.CodStatus = (int)statuscod;
                res.Obj = "";
                foreach(var error in result.Errors)
                {
                    res.Messagge.Add(error.ErrorMessage);
                }
                return res;
            }

            Usuario usr = _mapper.Map<Usuario>(usu);
            usr.Rols = new List<string>();
            _usuarios.InsertOne(usr);
            statuscod = StatusCodeEnum.Created;
            res.CodStatus = (int)statuscod;
            res.Obj = "";
            res.Success = true;
            res.Messagge.Add("Exito");
            return res;
        }

        public void Update(string email, Usuario u )
        {
            _usuarios.ReplaceOne(u=> u.Email == email, u);
        }

        public Response<string> SetRols(UserRolDto dto)
        {
            Response<string> res = new();
            res.Messagge = new List<string>();
            StatusCodeEnum statuscod = new();

            if (dto.Email != "")
            {
                var user = _usuarios.Find<Usuario>(u => u.Email == dto.Email).FirstOrDefault();
                if (user == null)
                {
                    statuscod = StatusCodeEnum.UserNotExist;
                    res.CodStatus = ((int)statuscod);
                    res.Messagge.Add(statuscod + " - No se ha encontrado el usuario");
                    res.Obj = "";
                    res.Success = false;
                    return res;
                }
                if(user.Password != dto.Password)
                {
                    statuscod = StatusCodeEnum.BadPassword;
                    res.CodStatus = (int)statuscod;
                    res.Messagge.Add(statuscod + " - Password incorrecta");
                    res.Obj = "";
                    res.Success = false;
                    return res;
                }
               if (dto.Rols.Count == 0)
                {
                    statuscod = StatusCodeEnum.BadRequest;
                    res.CodStatus = (int)statuscod;
                    res.Messagge.Add("No hay roles para agregar ");
                    res.Obj = "";
                    res.Success = false;
                    return res;
                }

                foreach(var item in dto.Rols)
                {
                    if(!user.Rols.Contains(item)) user.Rols.Add(item);
                }
                
                Update(dto.Email, user);
                statuscod = StatusCodeEnum.OK;
                res.CodStatus = (int)statuscod;
                res.Obj = "";
                res.Success = true;
                res.Messagge.Add("Exito");
                return res;
            }
            statuscod = StatusCodeEnum.BadRequest;
            res.CodStatus = (int)statuscod;
            res.Messagge.Add("error ");
            res.Obj = "";
            res.Success = false;
            return res;
        }

        public void Delete(string email)
        {
            _usuarios.DeleteOne(u => u.Email == email);
        }

        public Response<string> DeleteRol(UserRolDto dto)
        {
            Response<string> res = new();
            res.Messagge = new List<string>();
            StatusCodeEnum statuscod = new();
            if (dto.Email != "")
            {
                var user = _usuarios.Find<Usuario>(u => u.Email == dto.Email).FirstOrDefault();
                if (user == null)
                {
                    statuscod = StatusCodeEnum.UserNotExist;
                    res.CodStatus = ((int)statuscod);
                    res.Messagge.Add(statuscod + " - No se ha encontrado el usuario");
                    res.Obj = "";
                    res.Success = false;
                    return res;
                }
                if (user.Password != dto.Password)
                {
                    statuscod = StatusCodeEnum.BadPassword;
                    res.CodStatus = (int)statuscod;
                    res.Messagge.Add(statuscod + " - Password incorrecta");
                    res.Obj = "";
                    res.Success = false;
                    return res;
                }
                if (user.Rols.Count == 0 || dto.Rols.Count == 0)
                {
                    statuscod = StatusCodeEnum.RolError;
                    res.CodStatus = (int)statuscod;
                    foreach (var item in dto.Rols) 
                    {
                        res.Messagge.Add(item+" No pudo ser eliminado");
                    }
                    if(dto.Rols.Count == 0 ) res.Messagge.Add(" No se han ingresado rol/es para eliminar ");
                    res.Obj = "";
                    res.Success = false;
                    return res;
                }
                bool error = false;
                foreach (var item in dto.Rols)
                {
                    if (!user.Rols.Contains(item))
                    {
                        res.Messagge.Add(item + " No pudo ser eliminado");
                        error = true;
                    }
                    if (user.Rols.Contains(item)) user.Rols.Remove(item);
                }
                
                Update(dto.Email, user);
                if(!error == true)
                {
                    res.Messagge.Add("Exito");
                    statuscod = StatusCodeEnum.OK;
                    res.CodStatus = (int)statuscod;
                }
                else
                {
                    statuscod = StatusCodeEnum.RolError;
                    res.CodStatus = (int)statuscod;
                }
                res.Obj = "";
                res.Success = true;
                return res;
            }
            statuscod = StatusCodeEnum.BadRequest;
            res.CodStatus = (int)statuscod;
            res.Messagge.Add("Error ");
            res.Obj = "";
            res.Success = false;
            return res;
        }

        public bool Login(string email, string pass)
        {
            var user = _usuarios.Find<Usuario>(u => u.Email == email).FirstOrDefault();
            if(user == null) return false;
            if (user.Email == email && user.Password == pass) return true;
            return false;
        }
    }
}
