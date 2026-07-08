using AutoEscola.API.BLL.Interface.Base;
using AutoEscola.API.Models.DTO.Instrutor;
using AutoEscola.API.Models.ViewModel.Instrutor;

namespace AutoEscola.API.BLL.Interface
{
    public interface IInstrutorDisponivel : IBaseBLL<InstrutorDisponivelDTO>
    {
        Task<List<InstrutorDisponivelViewModel>> BuscarInstrutorDisponivel();
    }
}
