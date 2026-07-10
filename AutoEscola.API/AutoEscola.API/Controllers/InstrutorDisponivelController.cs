using AutoEscola.API.BLL;
using AutoEscola.API.BLL.Interface;
using AutoEscola.API.Data;
using AutoEscola.API.Enum;
using AutoEscola.API.Models.DTO.Instrutor;
using AutoEscola.API.Models.DTO.Usuario;
using AutoEscola.API.Models.ViewModel.Instrutor;
using AutoEscola.API.Models.ViewModel.Usuario;
using AutoEscola.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoEscola.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] 
    public class InstrutorDisponivelController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IInstrutorDisponivel _instrutorDisponivelBll;

        public InstrutorDisponivelController(IInstrutorDisponivel instrutorDisponivel)
        {
            _instrutorDisponivelBll = instrutorDisponivel;
        }

        [HttpGet()]
        public async Task<IActionResult> BuscarInstrutoresDisponiveis([FromServices] JwtService jwtService)
        {
            try
            {
                var resultado = await _instrutorDisponivelBll.BuscarInstrutorDisponivel();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpGet("{usuarioId}")]
        public async Task<IActionResult> BuscarInstrutorDisponivel(int usuarioId, [FromServices] JwtService jwtService)
        {
            try
            {
                var resultado = await _instrutorDisponivelBll.BuscarPorId(usuarioId);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpPost("atualizar/status")]
        public async Task<IActionResult> AtualizarStatusInstrutor(InstrutorDisponivelDTO instrutorDisponivel)
        {
            try
            {
                var resultado = await _instrutorDisponivelBll.Atualizar(instrutorDisponivel);

                var novoUsuario = new InstrutorDisponivelViewModel
                {
                    Id = resultado.Id,    
                    InstrutorId = resultado.InstrutorId,
                    DataAula = resultado.DataAula,
                    Status = resultado.Status  
                }; 

                return Ok(novoUsuario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


    }
}
