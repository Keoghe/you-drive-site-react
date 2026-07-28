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
    public class ConfiguracaoAcessoController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IConfiguracaoAcesso _configuracaoAcessoBll;


        public ConfiguracaoAcessoController(IConfiguracaoAcesso configuracaoAcessoBll)
        {
            _configuracaoAcessoBll = configuracaoAcessoBll;
        }
         
        [Authorize]
        [HttpGet("usuarioId/{usuarioId}")]
        public async Task<IActionResult> BuscarConfigurcaoAcessoUsuario(int usuarioId, [FromServices] JwtService jwtService)
        {
            try
            {
                var usuarios = await _configuracaoAcessoBll.BuscarConfigurcaoAcessoUsuario(usuarioId);
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}
