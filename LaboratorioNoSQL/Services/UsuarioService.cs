using LaboratorioNoSQL.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaboratorioNoSQL.Services
{
    public class UsuarioService
    {
        private IMongoCollection<Usuario> _usuarios;

        public UsuarioService(IConexion conexion) //Obtengo el inject que hice en el startup
        {
            var client = new MongoClient(conexion.Server); //Crea la conexion 
            var database = client.GetDatabase(conexion.Database); // obtiene la base de datos
            _usuarios = database.GetCollection<Usuario>(conexion.Collection); // instancia la coleccion 
        }

        public List<Usuario> Get()
        {
            return _usuarios.Find(d => true).ToList();
        }

        public Usuario Post(Usuario usu)
        {
            _usuarios.InsertOne(usu);
            return usu;
        }

        public void Update(string email, Usuario u )
        {
            _usuarios.ReplaceOne(u=> u.Email == email, u);
        }

        public void Delete(string email)
        {
            _usuarios.DeleteOne(u => u.Email == email);
        }

    }
}
