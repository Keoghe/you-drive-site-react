using AutoEscola.API.BLL;
using AutoEscola.API.Data;
using AutoEscola.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace AutoEscola.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var usuarios = _context.Usuarios
                .Where(u => !u.Excluido)
                .ToList();

            return Ok(usuarios);
        }

        [HttpPost]
        public IActionResult CriarUsuario(UsuarioDTO novoUsuario)
        {
            try
            {
                var usuarioBll = new UsuariosBLL(_context);

                var usuario = usuarioBll.CriarUsuario(novoUsuario);

                return Ok(usuario);
            }
            catch (Exception)
            {

                throw;
            }

            
        }
    }
}
