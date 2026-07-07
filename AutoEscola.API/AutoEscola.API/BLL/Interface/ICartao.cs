using AutoEscola.API.BLL.Interface.Base;
using AutoEscola.API.Models.DTO.Cartao;
using AutoEscola.API.Models.ViewModel.Conta;

namespace AutoEscola.API.BLL.Interface
{
    public interface ICartao : IBaseBLL<CartaoDTO>
    {
        Task<CartaoViewModel> AdicionarCartao(CartaoDTO endereco);
    }
}
