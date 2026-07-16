using AutoEscola.API.BLL;
using AutoEscola.API.BLL.Interface;
using AutoEscola.API.Data;
using AutoEscola.API.Enum;
using AutoEscola.API.Models.DTO.Instrutor;
using AutoEscola.API.Models.DTO.Usuario;
using AutoEscola.API.Models.ViewModel.Usuario;
using AutoEscola.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoEscola.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] 
    public class InstrutorController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IInstrutor _instrutorBll;

        public InstrutorController(IInstrutor instrutor)
        {
            _instrutorBll = instrutor;
        }

        [HttpPut("atualizar")]
        public async Task<IActionResult> AlterarStatusInstrutor(InstrutorDTO instrutorDTO)
        {
            try
            { 
                var instrutor = await _instrutorBll.Atualizar(instrutorDTO); 

                return Ok(instrutor);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            } 
        }

        [HttpPut("atualizar/localizacao")]
        public async Task<IActionResult> AlterarLocalizacaoInstrutor(InstrutorDTO instrutorDTO)
        {
            try
            {
                var instrutor = await _instrutorBll.AtualizarLocalizacao(instrutorDTO);

                return Ok(instrutor);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{instrutorId}")]
        public async Task<IActionResult> BuscarInstrutor(int instrutorId, [FromServices] JwtService jwtService)
        {
            try
            {
                var resultado = await _instrutorBll.BuscarPorId(instrutorId);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }
         

    }
}
