using AutoEscola.API.BLL.Interface.Base;
using AutoEscola.API.Enum;
using AutoEscola.API.Models.DTO.Aula;
using AutoEscola.API.Models.DTO.Notificacao;
using AutoEscola.API.Models.ViewModel.Notific_acoes;

namespace AutoEscola.API.BLL.Interface
{
    public interface INotificaoAula : IBaseBLL<NotificacaoAulaDTO>
    {
        Task<NotificacaoAulaViewModel> AdicionarNovaNotificacao(NotificacaoAulaDTO entidade); 
        Task<List<NotificacaoAulaDTO>> BuscarNotificaoInstrutor(int instrutorId, StatusNotificacaoAula statusNoticacaoAula);
        Task<List<NotificacaoAulaDTO>> BuscarNotificaoAluno(int alunoId);
        Task<NotificacaoAulaViewModel> AtualizarStatusNotificaoInstrutor(AlterarStatusNotificacaoAulaDTO alterarStatusNotificacaoAula);
        Task<NotificacaoAulaViewModel> AtualizarPosicaoUsuario(AtualizarPosicaoUsuarioDTO atualizarPosicaoUsuario);
        Task<CancelamentoNotificacaoAulaViewModel> CancelarAulaAluno(NotificacaoAulaDTO notificacaoAula);
    }
}
