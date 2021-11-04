using System;
using System.Collections.Generic;
using System.Net;

namespace LaboratorioNoSQL.Dtos
{
    public class Response<T>
    {
        public T Obj { get; set; }
        public Boolean Success { get; set; }
        public int CodStatus { get; set; }
        public List<string> Messagge { get; set; }

    }
}
