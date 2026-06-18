

using AutoEscola.API.BLL;
using AutoEscola.API.BLL.Interface;
using AutoEscola.API.Data;
using AutoEscola.API.Models.DTO.Aula;
using AutoEscola.API.Models.DTO.Documento;
using AutoEscola.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoEscola.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentoController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IDocumento _documentoBll;


        public DocumentoController(IDocumento documentoBll)
        {
            _documentoBll = documentoBll;
        }

        [Authorize]
        [HttpPost("upload/ativar/conta/instrutor")]
        public async Task<IActionResult> UploadArquivosContaInstrutor([FromBody] List<DocumentoDTO> listaArquivos, [FromServices] JwtService jwtService)
        {
            try
            { 
                var resultado = await _documentoBll.UploadAtivarContaInstrutor(listaArquivos);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }

        }

    }
}
