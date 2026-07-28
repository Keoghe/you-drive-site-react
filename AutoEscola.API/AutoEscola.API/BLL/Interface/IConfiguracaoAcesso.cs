using AutoEscola.API.BLL.Interface.Base;
using AutoEscola.API.Models.DTO.ControleAcesso;

namespace AutoEscola.API.BLL.Interface
{
    public interface IConfiguracaoAcesso : IBaseBLL<ConfiguracaoAcessoDTO>
    { 
        Task<List<ConfiguracaoAcessoDTO>> BuscarConfigurcaoAcessoUsuario(int usuarioId);
    }
}
