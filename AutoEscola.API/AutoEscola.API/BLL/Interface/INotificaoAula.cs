using AutoEscola.API.BLL.Interface.Base;
using AutoEscola.API.Models.DTO.Notificacao;
using AutoEscola.API.Models.ViewModel.Notific_acoes;

namespace AutoEscola.API.BLL.Interface
{
    public interface INotificaoAula : IBaseBLL<NotificacaoAulaDTO>
    {
        Task<NotificacaoAulaViewModel> AdicionarNovaNotificacao(NotificacaoAulaDTO entidade);

        Task<List<NotificacaoAulaDTO>> BuscarNotificaoInstrutor(int instrutorId);
    }
}
