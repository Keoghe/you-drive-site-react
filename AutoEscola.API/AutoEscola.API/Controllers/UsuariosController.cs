using AutoEscola.API.BLL;
using AutoEscola.API.BLL.Interface;
using AutoEscola.API.Data;
using AutoEscola.API.Models.DTO.Login;
using AutoEscola.API.Models.DTO.Usuario;
using AutoEscola.API.Models.ViewModel.Usuario;
using AutoEscola.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoEscola.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IUsuarios _usuariosBll;

        public UsuariosController(IUsuarios usuariosBll)
        {
            _usuariosBll = usuariosBll;
        }

        //public UsuariosController(AppDbContext context)
        //{
        //    _context = context;
        //}

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO,  [FromServices] JwtService jwtService)
        {
            try
            {
                var resultado = await _usuariosBll.ValidarLogin(loginDTO, jwtService);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);  
            }

        }

        [Authorize]
        [HttpGet]
        public IActionResult BuscarUsuario()
        {
            var usuarios = _context.Usuarios
                .Where(u => !u.Excluido)
                .ToList();

            return Ok(usuarios);
        }

        [HttpPost]
        public async Task<IActionResult> CriarUsuario(UsuarioDTO cadastroUsuario)
        {
            try
            {
                var usuario = await _usuariosBll.CriarUsuario(cadastroUsuario);

                var novoUsuario = new UsuarioViewModel
                {
                    Id = usuario.Id,
                    Nome = usuario.Nome,
                    Cnh = usuario.Cnh,
                    Cpf = usuario.Cpf,
                    DataNascimento = usuario.DataNascimento,
                    Email = usuario.Email,
                    Login = usuario.Login,
                    Saldo = usuario.Saldo
                };

                return Ok(novoUsuario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
