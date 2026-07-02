using AutoEscola.API.BLL.Interface.Base;
using AutoEscola.API.Enum;
using AutoEscola.API.Models.DTO.Documento;
using AutoEscola.API.Models.DTO.Instrutor;
using AutoEscola.API.Models.ViewModel.Documento;

namespace AutoEscola.API.BLL.Interface
{
    public interface IDocumento : IBaseBLL<DocumentoDTO>
    {
        Task<List<DocumentoViewModel>> UploadAtivarContaInstrutor(List<DocumentoDTO> listaArquivos);
        Task<DocumentoViewModel> UploadArquivo(DocumentoDTO arquivo);
        Task<DocumentoViewModel> BuscarArquivo(int documentoId);
        Task<List<DocumentoViewModel>> BuscarArquivosUsuario(int usuarioId, int statusDocumento = (int)StatusDocumento.Pendente, bool transformePdf = false); 
        Task<DownloadArquivoViewModel> BaixarArquivo(int documentoId);
        Task<DocumentoDTO> AtualizarStatusDocumento(DocumentoDTO documento);
        Task<DadosAtivacaoContaDTO> AtivarContaInstrutor(DadosAtivacaoContaDTO dadosAtivacaoConta);


    }
}
