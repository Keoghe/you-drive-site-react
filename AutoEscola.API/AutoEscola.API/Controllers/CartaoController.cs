using AutoEscola.API.BLL;
using AutoEscola.API.BLL.Interface;
using AutoEscola.API.Data;
using AutoEscola.API.Models.DTO.Cartao;
using AutoEscola.API.Models.DTO.Endereco;
using AutoEscola.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoEscola.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartaoController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ICartao _cartaoBll;

        public CartaoController(AppDbContext context, ICartao cartaoBll)
        {
            _context = context;
            _cartaoBll = cartaoBll;
        }

        [HttpGet("{usuarioId}")]
        public async Task<IActionResult> BuscarCartao(int usuarioId, [FromServices] JwtService jwtService)
        {
            try
            {
                var resultado = await _cartaoBll.BuscarTodos(usuarioId);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpPost()]
        public async Task<IActionResult> CadastrarCartao ([FromBody] CartaoDTO cartao, [FromServices] JwtService jwtService)
        {
            try
            {
                var resultado = await _cartaoBll.AdicionarCartao(cartao);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }


    }
}
