using AutoEscola.API.BLL.Interface.Base;
using AutoEscola.API.Models.DTO.Instrutor;
using AutoEscola.API.Models.Entidade;
using AutoEscola.API.Models.ViewModel.Endereco;

namespace AutoEscola.API.BLL.Interface
{
    public interface IEndereco : IBaseBLL<Endereco>
    {
        Task<EnderecoViewModel> AdicionarEndereco(EnderecoDTO endereco);
    }
}
