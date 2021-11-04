using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaboratorioNoSQL.Dtos
{
    public class UserRolDto
    {
        public string Email {  get; set; }  
        public string Password {  get; set; }
        public ICollection<string> Rols { get; set; }
    }
}
