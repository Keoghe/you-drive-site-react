using AutoEscola.API.BLL;
using AutoEscola.API.BLL.Interface;
using AutoEscola.API.Data;
using AutoEscola.API.Models.DTO.Aula;
using AutoEscola.API.Models.DTO.Login;
using AutoEscola.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoEscola.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AulasController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IAulas _aulasBll;


        public AulasController(IAulas aulasBll)
        {
            _aulasBll = aulasBll;
        }

        [HttpPost("cadastrar")]
        public async Task<IActionResult> CadastrarAula(AulaDTO aulaDTO, [FromServices] JwtService jwtService)
        {
            try
            {
                var resultado = await _aulasBll.CriarAula(aulaDTO);

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
    }
}
