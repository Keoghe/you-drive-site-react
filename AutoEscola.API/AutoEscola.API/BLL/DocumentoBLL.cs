using AutoEscola.API.BLL.Interface;
using AutoEscola.API.Data;
using AutoEscola.API.Enum;
using AutoEscola.API.Models.DTO.Documento;
using AutoEscola.API.Models.ViewModel.Documento;
using AutoEscola.API.Services;

namespace AutoEscola.API.BLL
{
    public class DocumentoBLL : IDocumento
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;
        private readonly IHttpContextAccessor _httpContext;

        public DocumentoBLL(AppDbContext context, JwtService jwtService, IHttpContextAccessor httpContext)
        {
            _context = context;
            _jwtService = jwtService;
            _httpContext = httpContext;
        }
        public Task<DownloadArquivoViewModel> BaixarArquivo(int documentoId)
        {
            throw new NotImplementedException();
        }

        public Task<DocumentoViewModel> BuscarArquivo(int documentoId)
        {
            throw new NotImplementedException();
        }

        public Task<List<DocumentoViewModel>> BuscarArquivosUsuario(int usuarioId)
        {
            throw new NotImplementedException();
        }

        public Task<DocumentoViewModel> UploadArquivo(DocumentoDTO arquivo)
        {
            throw new NotImplementedException();
        }

        public async Task<List<DocumentoViewModel>> UploadAtivarContaInstrutor(List<DocumentoDTO> listaArquivos)
        {
            ValidarArquivoEnviados(listaArquivos);

            try
            {
                var entidades = listaArquivos.Select(a => new Models.Entidade.Documento
                {
                    NomeOriginal = a.NomeOriginal,
                    CaminhoArquivo = a.CaminhoArquivo,
                    TipoDocumentoId = a.TipoDocumentoId,
                    Status = (int)StatusDocumento.Pendente,
                    DataCriacao = DateTime.UtcNow
                }).ToList();

                // ✅ adiciona no contexto
                _context.Documentos.AddRange(entidades);
                 
                await _context.SaveChangesAsync();
                 
                var resultado = entidades.Select(e => new DocumentoViewModel
                {
                    Id = e.Id,
                    NomeOriginal = e.NomeOriginal,
                    Status = e.Status,
                    DataCriacao = e.DataCriacao
                }).ToList();

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao importar arquivos: " + ex.Message);
            }
        }

        private void ValidarArquivoEnviados(List<DocumentoDTO> listaArquivos)
        {
            var cpf = _context.Usuarios
                .Where(u => u.Id == listaArquivos.FirstOrDefault().usuarioId && u.Excluido == (int)StatusContaUsuario.ATIVO)
                .Select(u => u.Cpf).FirstOrDefault();

            if (listaArquivos != null && listaArquivos.Count > 0)
            {
                foreach (var arquivo in listaArquivos)
                {
                    switch (arquivo.TipoDocumentoId)
                    {
                        case (int)TipoAnexo.CNH:
                            if (arquivo.Base64 == null || arquivo.Base64.Length == 0)
                                throw new Exception("Documento CNH não foi enviado");
                            break;
                        case (int)TipoAnexo.CREDENCIA_CERTIFICADO_AUTONOMO:
                            if (arquivo.Base64 == null || arquivo.Base64.Length == 0)
                                throw new Exception("Documento Credencial/Certificado de Autônomo não foi enviado");
                            break;
                        case (int)TipoAnexo.COMPROVANTE_ENDERECO:
                            if (arquivo.Base64 == null || arquivo.Base64.Length == 0)
                                throw new Exception("Documento Comprovante de Endereço não foi enviado");
                            break;
                        case (int)TipoAnexo.CERTIDAO_ANTECEDENTE_CRIMINAL:
                            if (arquivo.Base64 == null || arquivo.Base64.Length == 0)
                                throw new Exception("Documento Certidão de Antecedente Criminal não foi enviado");
                            break;
                        default:
                            break;
                    }


                }
            }
            else
            {
                throw new Exception("Nenhum arquivo enviado");
            }
        }
    }
}
