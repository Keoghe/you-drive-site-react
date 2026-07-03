using AutoEscola.API.BLL;
using AutoEscola.API.BLL.Interface;
using AutoEscola.API.Data;
using AutoEscola.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoEscola.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VeiculoController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IVeiculo _veiculoBll;

        public VeiculoController(AppDbContext context, IVeiculo veiculoBll)
        {
            _context = context;
            _veiculoBll = veiculoBll;
        }

        [HttpGet("{usuarioId}")]
        public async Task<IActionResult> BuscarVeiculoInstrutor(int usuarioId, [FromServices] JwtService jwtService)
        {
            try
            {
                var resultado = await _veiculoBll.BuscarVeiculoInstrutor(usuarioId);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}
