using AutoEscola.API.BLL;
using AutoEscola.API.BLL.Interface;
using AutoEscola.API.Data;
using AutoEscola.API.Models.DTO.Documento;
using AutoEscola.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoEscola.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TiposDocumentoController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ITiposDocumento _tiposDocumentoBll;

        public TiposDocumentoController(ITiposDocumento tiposDocumentoBll)
        {
            _tiposDocumentoBll = tiposDocumentoBll;
        }
         
        [Authorize]
        [HttpGet("{tipoUsuarioId}")]
        public async Task<IActionResult> BuscarTiposDocumento(int tipoUsuarioId, [FromServices] JwtService jwtService)
        {
            try
            {
                var resultado = await _tiposDocumentoBll.BuscarTiposDocumento(tipoUsuarioId);

                return Ok(resultado);  
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            } 
        }        
    }
}
