using AutoEscola.API.Models.DTO.Aula;
using AutoEscola.API.Models.Entidade;

namespace AutoEscola.API.BLL.Interface
{
    public interface IAulas
    {
        Task<Aula> CriarAula(AulaDTO novaAula);
        Task<Aula> BuscarAulaPorId(int AulaId);
        Task<List<Aula>> BuscarAulasPorId(List<int> AulaId);
        Task<List<Aula>> BuscarAulas();
        Task<bool> AtualizarAulaPorId(AulaDTO AulaId);
    }
}
