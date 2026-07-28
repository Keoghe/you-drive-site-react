using AutoEscola.API.BLL;
using AutoEscola.API.BLL.Interface;
using AutoEscola.API.Data;
using AutoEscola.API.Enum;
using AutoEscola.API.Models.DTO.Instrutor;
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
    public class UsuariosController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IUsuarios _usuariosBll;
        private readonly IInstrutor _instrutorBll;
        private readonly IGrupoUsuario _grupoUsuarioBll;

        public UsuariosController(IUsuarios usuariosBll, IInstrutor instrutorBll, IGrupoUsuario grupoUsuarioBll)
        {
            _usuariosBll = usuariosBll;
            _instrutorBll = instrutorBll;
            _grupoUsuarioBll = grupoUsuarioBll;
        }

        //public UsuariosController(AppDbContext context)
        //{
        //    _context = context;
        //}


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            try
            {
                var resultado = await _usuariosBll.ValidarLogin(loginDTO);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }

        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> BuscarUsuario()
        {
            try
            {
                var resultado = await _usuariosBll.BuscarUsuarios();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpPost("cadastrar")]
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
                    Saldo = usuario.Saldo,
                };
                var grupoUsuario = new Models.DTO.Grupo.GrupoUsuarioDTO
                {
                    UsuarioId = usuario.Id,
                    GrupoId = (int)TipoUsuario.Condutor
                };
                if (cadastroUsuario.TipoUsuario == (int)TipoUsuario.Instrutor)
                {

                    var novoInstrutor = new InstrutorDTO
                    {
                        UsuarioId = usuario.Id
                    };

                    await _instrutorBll.Adicionar(novoInstrutor);

                    grupoUsuario.GrupoId = (int)TipoUsuario.Instrutor;                    
                }

                await _grupoUsuarioBll.Adicionar(grupoUsuario);
                 

                return Ok(novoUsuario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [Authorize]
        [HttpGet("buscar-usuario/{usuarioId}")]
        public async Task<IActionResult> BuscarUsuario(int usuarioId)
        {
            try
            {
                var usuario = await _usuariosBll.BuscarUsuarioPorId(usuarioId);


                if (usuario == null)
                    throw new Exception("Usuário não encontrado");

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [Authorize]
        [HttpGet("buscar-dados/minha-conta/{usuarioId}")]
        public async Task<IActionResult> BuscarDadosMinhaConta(int usuarioId)
        {
            try
            {
                var usuario = await _usuariosBll.BuscarDadosMinhaConta(usuarioId);

                if (usuario == null || usuario.Count() == 0)
                    throw new Exception("Usuário não encontrado");

                return Ok(usuario.First());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [Authorize]
        [HttpGet("buscar-instrutores/{pagina}/{tamanhoPagina}")]
        public async Task<IActionResult> BuscarInstrutores(int pagina = 1, int tamanhoPagina = 10)
        {
            try
            {
                var instrutores = await _usuariosBll.BuscarInstrutores(pagina, tamanhoPagina);
                if (instrutores == null)
                    throw new Exception("Instrutores não encontrados.");

                return Ok(instrutores);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


    }
}
