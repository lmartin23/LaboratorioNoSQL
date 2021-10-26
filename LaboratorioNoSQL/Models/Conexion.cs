using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaboratorioNoSQL.Models
{
    public class Conexion : IConexion
    {
        public string Server {  get; set; }
        public string Database { get; set; }
        public string Collection { get; set; }
    }
    public interface IConexion
    {
        string Server { get; set; }
        string Database { get; set; }
        string Collection { get; set; }
    }
}
