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
    public class GrupoConfiguracaoAcessoController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IGrupoConfiguracaoAcesso _grupoConfiguracaoAcessoBll;


        public GrupoConfiguracaoAcessoController(IGrupoConfiguracaoAcesso grupoConfiguracaoAcessoBll)
        {
            _grupoConfiguracaoAcessoBll = grupoConfiguracaoAcessoBll;
        }

        [Authorize]
        [HttpGet("grupoId/{grupoId}")]
        public async Task<IActionResult> BuscarConfigurcaoAcessoGrupo(int grupoId, [FromServices] JwtService jwtService)
        {
            try
            {
                var usuarios = await _grupoConfiguracaoAcessoBll.BuscarConfigurcaoAcessoGrupo(grupoId);

                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}
