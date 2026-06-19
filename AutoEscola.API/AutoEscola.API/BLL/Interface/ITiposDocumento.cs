using AutoEscola.API.BLL.Interface.Base;
using AutoEscola.API.Models.Entidade;
using AutoEscola.API.Models.ViewModel;

namespace AutoEscola.API.BLL.Interface
{
    public interface ITiposDocumento : IBaseBLL<TiposDocumento>
    {
        public Task<List<TiposDocumentoViewModel>> BuscarTiposDocumento(int tipoUsuarioId);
    }
}
