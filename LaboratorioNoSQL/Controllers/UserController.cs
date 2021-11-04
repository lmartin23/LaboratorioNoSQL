using LaboratorioNoSQL.Dtos;
using LaboratorioNoSQL.Models;
using LaboratorioNoSQL.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

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
        public ActionResult<Response<String>> CreateUser(BaseUserDto usu)
        {
            return _usuarioService.Post(usu); ;
        }

        [HttpPut]
        public ActionResult<Response<String>> SetRols( UserRolDto u)
        {
            return _usuarioService.SetRols(u);
            
        }

        [HttpDelete]
        public ActionResult Delete(string email)
        {
            _usuarioService.Delete(email);
            return Ok();
        }
    }
}
