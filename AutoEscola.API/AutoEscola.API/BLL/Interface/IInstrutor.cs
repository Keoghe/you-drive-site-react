using AutoEscola.API.Models.DTO.Login;
using AutoEscola.API.Models.DTO.Instrutor;
using AutoEscola.API.Models.ViewModel.Login;
using AutoEscola.API.Models.Entidade;

namespace AutoEscola.API.BLL.Interface
{
    public interface IInstrutor
    {
        Task<Instrutor> CriarInstrutor(InstrutorDTO novoInstrutor);
        Task<Instrutor> BuscarInstrutorPorId(int instrutorId);
        Task<List<Instrutor>> BuscarInstrutoresPorId(List<int> instrutorId);
        Task<List<Instrutor>> BuscarInstrutores();
        Task<bool> AtualizarInstrutorPorId(List<int> instrutorId); 
    }
}
