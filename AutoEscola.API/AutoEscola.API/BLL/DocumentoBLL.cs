using AutoEscola.API.BLL.Interface;
using AutoEscola.API.Data;
using AutoEscola.API.Enum;
using AutoEscola.API.Models.DTO.Documento;
using AutoEscola.API.Models.Entidade;
using AutoEscola.API.Models.ViewModel.Documento;
using AutoEscola.API.Services;
using AutoEscola.API.Util;
using Microsoft.EntityFrameworkCore;
using System.Buffers.Text;
using System.IO;

namespace AutoEscola.API.BLL
{
    public class DocumentoBLL : IDocumento
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IStorage _storageBLL;
        private readonly ITiposDocumento _tiposDocumentoBLL;
        public DocumentoBLL(AppDbContext context, JwtService jwtService, IHttpContextAccessor httpContext, IStorage storageBLL, ITiposDocumento tiposDocumentoBLL)
        {
            _context = context;
            _jwtService = jwtService;
            _httpContext = httpContext;
            _storageBLL = storageBLL;
            _tiposDocumentoBLL = tiposDocumentoBLL;
        }
        public Task<DownloadArquivoViewModel> BaixarArquivo(int documentoId)
        {
            throw new NotImplementedException();
        }

        public Task<DocumentoViewModel> BuscarArquivo(int documentoId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<DocumentoViewModel>> BuscarArquivosUsuario(int usuarioId, int statusDocumento = (int)StatusDocumento.Pendente, bool transformePdf = false)
        {
            try
            {
                var conversor = new ConverterArquivos();
                var documentosUsuario = new List<DocumentoViewModel>();
                var documentos = await _context.Documentos.Where(c => c.UsuarioId == usuarioId && c.Excluido == statusDocumento).ToListAsync();

                foreach (var documento in documentos)
                {
                    var documentoBase64 = await File.ReadAllBytesAsync(documento.CaminhoArquivo);
                    var arquivoBase64 = Convert.ToBase64String(documentoBase64);
                    if (!documento.NomeOriginal.Contains(".pdf"))
                    {
                        arquivoBase64 = conversor.ConverterImagemBase64ParaPdfBase64(arquivoBase64);
                    } 

                    documentosUsuario.Add(new DocumentoViewModel
                    {
                        Id = documento.Id,
                        NomeOriginal = documento.NomeOriginal,
                        Status = documento.Status,
                        TipoDocumentalId = documento.TipoDocumentoId,
                        DataCriacao = documento.DataCriacao,
                        Descricao = documento.DescricaoAnalise, 
                        Base64 = arquivoBase64
                    });
                }

                return documentosUsuario;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
        }

        public Task<DocumentoViewModel> UploadArquivo(DocumentoDTO arquivo)
        {
            throw new NotImplementedException();
        }

        public async Task<List<DocumentoViewModel>> UploadAtivarContaInstrutor(List<DocumentoDTO> listaArquivos)
        {

            try
            {
                await ValidarDocumentoObrigatorios(listaArquivos, (int)TipoUsuario.Instrutor);

                if (await VerificarNovoDocumentoAnalise(listaArquivos) >= 0)
                {

                    listaArquivos = await ValidarArquivosEnviados(listaArquivos, (int)TipoUsuario.Instrutor);

                    var entidades = listaArquivos.Select(a => new Models.Entidade.Documento
                    {
                        UsuarioId = a.UsuarioId,
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
                else
                {
                    throw new Exception("Erro ao validar documentos.");
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<DocumentoDTO>> ValidarArquivosEnviados(List<DocumentoDTO> listaArquivos, int tipoUsuarioId)
        {

            var usuario = _context.Usuarios
                .Where(u => u.Id == listaArquivos.FirstOrDefault().UsuarioId && u.Excluido == (int)StatusContaUsuario.ATIVO)
                .FirstOrDefault();

            if (listaArquivos != null && listaArquivos.Count > 0)
            {
                foreach (var arquivo in listaArquivos)
                {
                    var nomeArquivo = $"";

                    switch (arquivo.TipoDocumentoId)
                    {
                        case (int)TipoAnexo.CNH:

                            nomeArquivo = "CNH";
                            if (arquivo.Base64 == null || arquivo.Base64.Length == 0)
                                throw new Exception("Documento CNH não foi enviado");
                            break;
                        case (int)TipoAnexo.CREDENCIA_CERTIFICADO_AUTONOMO:

                            nomeArquivo = "CREDENCIA_CERTIFICADO_AUTONOMO";
                            if (arquivo.Base64 == null || arquivo.Base64.Length == 0)
                                throw new Exception("Documento Credencial/Certificado de Autônomo não foi enviado");
                            break;
                        case (int)TipoAnexo.COMPROVANTE_ENDERECO:

                            nomeArquivo = "COMPROVANTE_ENDERECO";
                            if (arquivo.Base64 == null || arquivo.Base64.Length == 0)
                                throw new Exception("Documento Comprovante de Endereço não foi enviado");
                            break;
                        case (int)TipoAnexo.CERTIDAO_ANTECEDENTE_CRIMINAL:

                            nomeArquivo = "CERTIDAO_ANTECEDENTE_CRIMINAL";
                            if (arquivo.Base64 == null || arquivo.Base64.Length == 0)
                                throw new Exception("Documento Certidão de Antecedente Criminal não foi enviado");
                            break;
                        default:
                            break;
                    }

                    var caminho = await _storageBLL.BuscarTodos();

                    if (caminho != null && caminho.Count > 0)
                    {
                        var caminhoFisico = @$"{caminho?.FirstOrDefault()?.Caminho}\{usuario.Id}_{usuario.Cpf}";

                        if (!Path.Exists(caminhoFisico))
                        {
                            Directory.CreateDirectory(caminhoFisico);
                        }

                        byte[] bytes = Convert.FromBase64String(arquivo.Base64);

                        var extensao = ObterExtensaoArquivo(arquivo.NomeOriginal);

                        nomeArquivo += $".{extensao}";

                        var caminhoCompleto = Path.Combine(caminhoFisico, nomeArquivo);

                        arquivo.CaminhoArquivo = caminhoCompleto;

                        await File.WriteAllBytesAsync(caminhoCompleto, bytes);
                    }
                    else
                    {
                        throw new Exception("Erro ao salvar arquivo, verifique a configuração do storage");
                    }
                }

                return listaArquivos;
            }
            else
            {
                throw new Exception("Nenhum arquivo enviado");
            }
        }
        private async Task ValidarDocumentoObrigatorios(List<DocumentoDTO> listaArquivos, int tipoUsuarioId)
        {
            var usuarioId = listaArquivos.First().UsuarioId;

            if (usuarioId == 0)
                throw new Exception($"O usuário informado não existe");

            var tiposDocumento = await _tiposDocumentoBLL.BuscarTodos();

            var documentosSalvos = await _context.Documentos
                .Where(c =>
                    c.UsuarioId == usuarioId &&
                    c.Excluido == (int)Status.ATIVO
                )
                .ToListAsync();



            foreach (var documento in tiposDocumento.Where(c => c.Obrigatorio == (int)Status.ATIVO && c.TipoUsuarioId == tipoUsuarioId))
            {
                if (documentosSalvos.FindAll(c => c.TipoDocumentoId == documento.Id).Count > 0)
                {
                    continue;
                }

                if (listaArquivos.FindAll(c => c.TipoDocumentoId == documento.Id).Count == 0)
                {
                    throw new Exception($"O Documento {documento.Descricao} é obrigatório e não foi enviado");
                }
            }
        }
        private string ObterExtensaoArquivo(string nomeArquivo)
        {

            string extensao = Path.GetExtension(nomeArquivo);

            return extensao.Replace(".", "");
        }

        private bool ValidarExtensaoArquivo(string nomeArquivo)
        {
            var extensao = ObterExtensaoArquivo(nomeArquivo);
            var extensoesPermitidas = new List<string> { "jpg", "jpeg", "png", "pdf" };
            return extensoesPermitidas.Contains(extensao.ToLower());
        }

        private async Task<int> VerificarNovoDocumentoAnalise(List<DocumentoDTO> listaArquivos)
        {
            var tiposIds = listaArquivos.Select(a => a.TipoDocumentoId).ToList();


            var documentosAtuais = await _context.Documentos
                .Where(d => d.UsuarioId == listaArquivos.First().UsuarioId
                         && tiposIds.Contains(d.TipoDocumentoId)
                         && d.Excluido == 0)
                .ToListAsync();


            foreach (var doc in documentosAtuais)
            {
                doc.Excluido = 1;
            }
            var retorno = await _context.SaveChangesAsync();

            return retorno;
        }


        public async Task<DocumentoDTO> AtualizarStatusDocumento(DocumentoDTO documento)
        {
            try
            {
                var entidade = await _context.Documentos
                        .FirstOrDefaultAsync(d => d.Id == documento.Id);

                if (entidade == null)
                    throw new Exception("Documento não encontrado");

                entidade.Status = documento.Status;
                entidade.DescricaoAnalise = documento.DescricaoAnalise;


                await _context.SaveChangesAsync();

                return new DocumentoDTO
                {
                    UsuarioId = entidade.UsuarioId,
                    TipoDocumentoId = entidade.TipoDocumentoId,
                    Status = entidade.Status
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public Task<DocumentoDTO> Adicionar(DocumentoDTO entidade)
        {
            throw new NotImplementedException();
        }

        public Task<DocumentoDTO> Atualizar(DocumentoDTO entidade)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Remover(int id)
        {
            throw new NotImplementedException();
        }

        public Task<DocumentoDTO> BuscarPorId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<DocumentoDTO>> BuscarTodos()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
