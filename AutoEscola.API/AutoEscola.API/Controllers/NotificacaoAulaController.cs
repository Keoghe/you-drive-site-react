using AutoEscola.API.BLL;
using AutoEscola.API.BLL.Interface;
using AutoEscola.API.Data;
using AutoEscola.API.Enum;
using AutoEscola.API.Models.DTO.Instrutor;
using AutoEscola.API.Models.DTO.Notificacao;
using AutoEscola.API.Models.ViewModel.Instrutor;
using AutoEscola.API.Models.ViewModel.Notific_acoes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoEscola.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotificacaoAulaController : Controller
    {
        private readonly AppDbContext _context;
        private readonly INotificaoAula _notificaoAulaBll;

        public NotificacaoAulaController(INotificaoAula notificaoAula)
        {
            _notificaoAulaBll = notificaoAula;
        }


        [HttpPost("adicionar")]
        public async Task<IActionResult> AdicionarNotificacao(NotificacaoAulaDTO notificacaoAula)
        {
            try
            {
                var resultado = await _notificaoAulaBll.Adicionar(notificacaoAula);
                var novaNotificacao = new NotificacaoAulaViewModel();
                if (resultado != null)
                {
                    novaNotificacao = new NotificacaoAulaViewModel
                    {
                        Id = resultado.Id,
                        AlunoId = resultado.AlunoId,
                        InstrutorId = resultado.InstrutorId,
                        Descricao = resultado.Descricao,
                        DataSolicitacao = resultado.DataSolicitacao,
                        Status = resultado.Status,
                        Excluido = resultado.Excluido
                    }; 
                }

                return Ok(novaNotificacao);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{notificacaoId}")]
        public async Task<IActionResult> BuscarNotificacao(int notificacaoId)
        {
            try
            {
                var resultado = await _notificaoAulaBll.BuscarPorId(notificacaoId);
                var novaNotificacao = new NotificacaoAulaViewModel();
                if (resultado != null)
                {
                    novaNotificacao = new NotificacaoAulaViewModel
                    {
                        Id = resultado.Id,
                        AlunoId = resultado.AlunoId,
                        InstrutorId = resultado.InstrutorId,
                        Descricao = resultado.Descricao,
                        DataSolicitacao = resultado.DataSolicitacao,
                        Status = resultado.Status,
                        Excluido = resultado.Excluido
                    };
                }

                return Ok(novaNotificacao);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("atualizar")]
        public async Task<IActionResult> AtualizarStatusNotificacao(AlterarStatusNotificacaoAula alterarStatusNotificacaoAula)
        {
            try
            {
                var notificacao = await _notificaoAulaBll.AtualizarStatusNotificaoInstrutor(alterarStatusNotificacaoAula);
              
                return Ok(notificacao);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("instrutor/{instrutorId}/pendente")]
        public async Task<IActionResult> BuscarNotificacaoInstrutor(int instrutorId)
        {
            try
            {
                var resultado = await _notificaoAulaBll.BuscarNotificaoInstrutor(instrutorId, StatusNotificacaoAula.Pendente);
                var novaNotificacao = new List<NotificacaoAulaViewModel>();
                if (resultado != null)
                {
                    foreach (var item in resultado)
                    {
                        novaNotificacao.Add(new NotificacaoAulaViewModel
                        {
                            Id = item.Id,
                            AlunoId = item.AlunoId,
                            InstrutorId = item.InstrutorId,
                            Latitude = item.Latitude,
                            Longitude = item.Longitude,
                            Descricao = item.Descricao,
                            DataSolicitacao = item.DataSolicitacao,
                            Status = item.Status,
                            Excluido = item.Excluido
                        });
                    } 
                }

                return Ok(novaNotificacao);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
