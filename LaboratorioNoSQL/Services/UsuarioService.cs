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
                res.CodStatus = ((int)HttpStatusCode.BadRequest);
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
            res.CodStatus = ((int)HttpStatusCode.Created);
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
                    res.CodStatus = (int)HttpStatusCode.BadRequest;
                    res.Messagge.Add("No hay roles para agregar ");
                    res.Obj = "";
                    res.Success = false;
                    return res;
                }
                var user1 = new Usuario
                {
                    Id = user.Id,
                    Email = user.Email,
                    Name = user.Name,
                    Password = user.Password,
                    LastName = user.LastName,
                };

                if(user.Rols.Count == 0) user1.Rols = dto.Rols;
                else
                {
                    foreach(var item in dto.Rols)
                    {
                        user.Rols.Add(item);
                    }
                    user1.Rols = user.Rols;
                }

                Update(dto.Email, user1);
                res.CodStatus = ((int)HttpStatusCode.Created);
                res.Obj = "";
                res.Success = true;
                res.Messagge.Add("Exito");
                return res;
            }
            res.CodStatus = (int)HttpStatusCode.BadRequest;
            res.Messagge.Add("error ");
            res.Obj = "";
            res.Success = false;
            return res;

        }

        public void Delete(string email)
        {
            _usuarios.DeleteOne(u => u.Email == email);
        }

    }
}
