using LaboratorioNoSQL.Models;
using LaboratorioNoSQL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaboratorioNoSQL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public UsuarioService _usuarioService;

        public UserController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;   
        }

        [HttpGet]
        public ActionResult<List<Usuario>> GetUsuarios()
        {
            return _usuarioService.Get(); 
        }
        [HttpPost]
        public ActionResult<Usuario> CreateUser(Usuario usu)
        {
            _usuarioService.Post(usu);
            return Ok(usu);
        }

        [HttpPut]
        public ActionResult Update(string email, Usuario u)
        {
            _usuarioService.Update(email, u);
            return Ok();
        }

        [HttpDelete]
        public ActionResult Delete(string email)
        {
            _usuarioService.Delete(email);
            return Ok();
        }
    }
}
