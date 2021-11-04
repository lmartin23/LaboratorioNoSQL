using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaboratorioNoSQL.Models
{
    public class Usuario
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id {  get; set; }
        [BsonElement("email")]
        public string Email { get; set; }
        [BsonElement("password")]
        public string Password { get; set; }
        [BsonElement("name")]
        public string Name {  get; set; }
        [BsonElement("lastname")]
        public string LastName {  get; set; }
        [BsonElement("rols")]
        public ICollection<string> Rols {  get; set; }
        
    }
}
//BsonElement le dice a Mongo como se llama tal elemento del Modelo en la Base