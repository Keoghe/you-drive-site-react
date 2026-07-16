using AutoEscola.API.Models.DTO.Login;
using AutoEscola.API.Models.DTO.Instrutor;
using AutoEscola.API.Models.ViewModel.Login;
using AutoEscola.API.Models.Entidade;
using AutoEscola.API.BLL.Interface.Base;

namespace AutoEscola.API.BLL.Interface
{
    public interface IInstrutor : IBaseBLL<InstrutorDTO>
    {
        Task<InstrutorDTO> AtualizarLocalizacao(InstrutorDTO instrutor); 
    }
}
