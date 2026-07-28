using AutoEscola.API.BLL.Interface.Base;
using AutoEscola.API.Models.DTO.ControleAcesso;

namespace AutoEscola.API.BLL.Interface
{
    public interface IGrupoConfiguracaoAcesso : IBaseBLL<GrupoConfiguracaoAcessoDTO>
    {
        Task<List<GrupoConfiguracaoAcessoDTO>> BuscarConfigurcaoAcessoGrupo(int grupoId);
    }
}
