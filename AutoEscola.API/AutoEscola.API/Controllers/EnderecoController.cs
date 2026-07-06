using AutoEscola.API.BLL;
using AutoEscola.API.BLL.Interface;
using AutoEscola.API.Data;
using AutoEscola.API.Models.DTO;
using AutoEscola.API.Models.DTO.Endereco;
using AutoEscola.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoEscola.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EnderecoController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IEndereco _enderecoBll;

        public EnderecoController(AppDbContext context, IEndereco enderecoBll)
        {
            _context = context;
            _enderecoBll = enderecoBll;
        }


        [HttpGet("{usuarioId}")]
        public async Task<IActionResult> BuscarEndereco(int usuarioId, [FromServices] JwtService jwtService)
        {
            try
            {
                var resultado = await _enderecoBll.BuscarEndereco(usuarioId);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpPost()]
        public async Task<IActionResult> CadastrarEndereco([FromBody] EnderecoDTO endereco, [FromServices] JwtService jwtService)
        {
            try
            {
                var resultado = await _enderecoBll.AdicionarEndereco(endereco);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }

    }
}
