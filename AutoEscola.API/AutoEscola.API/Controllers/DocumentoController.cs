

using AutoEscola.API.BLL;
using AutoEscola.API.BLL.Interface;
using AutoEscola.API.Data;
using AutoEscola.API.Models.DTO.Aula;
using AutoEscola.API.Models.DTO.Documento;
using AutoEscola.API.Models.DTO.Instrutor;
using AutoEscola.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoEscola.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentoController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IDocumento _documentoBll; 


        public DocumentoController(IDocumento documentoBll)
        {
            _documentoBll = documentoBll; 
        }

        [Authorize]
        [HttpPost("upload/ativar/conta/instrutor")]
        public async Task<IActionResult> AtivarContaInstrutor([FromBody] DadosAtivacaoContaDTO dadosAtivacaoConta, [FromServices] JwtService jwtService)
        {
            try
            { 
                var resultado = await _documentoBll.AtivarContaInstrutor(dadosAtivacaoConta); 

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }

        }

        [Authorize]
        [HttpGet("{usuarioId}/{statusDocumento}")]
        public async Task<IActionResult> BuscarTiposDocumento(int usuarioId, int statusDocumento, [FromServices] JwtService jwtService)
        {
            try
            {
                var resultado = await _documentoBll.BuscarArquivosUsuario(usuarioId, statusDocumento);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("buscar-documentos-analise/{usuarioId}/{statusDocumento}")]
        public async Task<IActionResult> BuscarDocumentosAnalise(int usuarioId, int statusDocumento, [FromServices] JwtService jwtService)
        {
            try
            {
                var resultado = await _documentoBll.BuscarArquivosUsuario(usuarioId, statusDocumento,true);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [Authorize]
        [HttpPut("")]
        public async Task<IActionResult> AtualizarStatusDocumento([FromBody] DocumentoDTO documento, [FromServices] JwtService jwtService)
        {
            try
            {
                var resultado = await _documentoBll.AtualizarStatusDocumento(documento);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }

        }
    }
}
