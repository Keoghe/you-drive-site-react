using AutoEscola.API.BLL.Interface.Base;
using AutoEscola.API.Models.DTO.Endereco;
using AutoEscola.API.Models.Entidade;
using AutoEscola.API.Models.ViewModel.Endereco;

namespace AutoEscola.API.BLL.Interface
{
    public interface IEndereco : IBaseBLL<Endereco>
    {
        Task<EnderecoViewModel> AdicionarEndereco(EnderecoDTO endereco);
        Task<EnderecoViewModel> BuscarEndereco(int usuarioId);
    }
}
