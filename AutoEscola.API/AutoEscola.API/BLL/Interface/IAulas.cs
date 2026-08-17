using AutoEscola.API.BLL.Interface.Base;
using AutoEscola.API.Enum;
using AutoEscola.API.Models.DTO.Aula;
using AutoEscola.API.Models.Entidade;

namespace AutoEscola.API.BLL.Interface
{
    public interface IAulas : IBaseBLL<AulaDTO>
    {
        Task<Aula> CriarAula(AulaDTO novaAula);
        Task<Aula> BuscarAulaPorId(int AulaId);
        Task<List<Aula>> BuscarAulas();
        Task<bool> AtualizarAulaPorId(AulaDTO AulaId);
        Task<List<AulaDTO>> BuscarAulasMes(int usuarioId, int mes, TipoUsuario tipoUsuario);
    }
}
