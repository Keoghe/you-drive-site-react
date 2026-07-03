using AutoEscola.API.Models.ViewModel.Documento;
using AutoEscola.API.Models.ViewModel.Endereco;
using AutoEscola.API.Models.ViewModel.Veiculo;

namespace AutoEscola.API.Models.ViewModel.Instrutor
{
    public class DadosAtivacaoContaViewModel
    { 
        public List<DocumentoViewModel> Documentos { get; set; } = new List<DocumentoViewModel>();  
        public VeiculoViewModel Veiculo { get; set; } = new VeiculoViewModel();
        public EnderecoViewModel Endereco { get; set; } = new EnderecoViewModel();
    }
}
