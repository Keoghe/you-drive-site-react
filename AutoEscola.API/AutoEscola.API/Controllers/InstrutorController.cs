using AutoEscola.API.BLL;
using AutoEscola.API.BLL.Interface;
using AutoEscola.API.Data;
using AutoEscola.API.Enum;
using AutoEscola.API.Models.DTO.Instrutor;
using AutoEscola.API.Models.DTO.Usuario;
using AutoEscola.API.Models.ViewModel.Usuario;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoEscola.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] 
    public class InstrutorController : ControllerBase
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

    }
}
