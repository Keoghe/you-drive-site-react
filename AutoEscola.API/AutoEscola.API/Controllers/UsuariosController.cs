using Microsoft.AspNetCore.Mvc;
using AutoEscola.API.Data;
using AutoEscola.API.Models;

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

            var usuario = new Usuario
            {
                Nome = novoUsuario.Nome,
                Cpf = novoUsuario.Cpf,
                Cnh = novoUsuario.Cnh,
                DataNascimento = novoUsuario.DataNascimento,
                Email = novoUsuario.Email,
                Senha = novoUsuario.Senha,
                Saldo = novoUsuario.Saldo,
                DataCadastro = DateTime.Now,
                Excluido = false
            };


            usuario.DataCadastro = DateTime.Now;
            usuario.Excluido = false;

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            return Ok(usuario);
        }
    }
}
