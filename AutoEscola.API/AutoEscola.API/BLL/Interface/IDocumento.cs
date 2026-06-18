using AutoEscola.API.Models.DTO.Documento;
using AutoEscola.API.Models.ViewModel.Documento;

namespace AutoEscola.API.BLL.Interface
{
    public interface IDocumento
    {
        Task<List<DocumentoViewModel>> UploadAtivarContaInstrutor(List<DocumentoDTO> listaArquivos);
        Task<DocumentoViewModel> UploadArquivo(DocumentoDTO arquivo);
        Task<DocumentoViewModel> BuscarArquivo(int documentoId);
        Task<List<DocumentoViewModel>> BuscarArquivosUsuario(int usuarioId); 
        Task<DownloadArquivoViewModel> BaixarArquivo(int documentoId);


    }
}
