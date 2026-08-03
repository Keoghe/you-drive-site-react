using AutoEscola.API.BLL.Interface.Base;
using AutoEscola.API.Models.DTO.Instrutor;
using AutoEscola.API.Models.ViewModel.Instrutor;
using AutoEscola.API.Models.ViewModel.Paginacao;

namespace AutoEscola.API.BLL.Interface
{
    public interface IInstrutorDisponivel : IBaseBLL<InstrutorDisponivelDTO>
    {
        Task<List<InstrutorDisponivelViewModel>> BuscarInstrutorDisponivel();
        Task<PaginacaoViewModel<InstrutorDisponivelCidadeViewModel>> BuscarInstrutorDisponivelCidade(string cidade, int pagina = 1, int tamanhoPagina = 10);
    }
}
