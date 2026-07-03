using AutoEscola.API.BLL.Interface.Base;
using AutoEscola.API.Models.DTO.Veiculo;
using AutoEscola.API.Models.Entidade;
using AutoEscola.API.Models.ViewModel.Veiculo;

namespace AutoEscola.API.BLL.Interface
{
    public interface IVeiculo : IBaseBLL<Veiculo>
    { 
        public Task<VeiculoViewModel> AdicionarVeiculo(VeiculoDTO veiculo);

        public Task<VeiculoViewModel> BuscarVeiculoInstrutor(int usuarioId);

    }
}
