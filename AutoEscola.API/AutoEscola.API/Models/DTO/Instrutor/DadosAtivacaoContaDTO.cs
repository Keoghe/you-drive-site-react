using AutoEscola.API.Models.DTO.Documento;
using AutoEscola.API.Models.Entidade;

namespace AutoEscola.API.Models.DTO.Instrutor
{
    public class DadosAtivacaoContaDTO
    {
        public EnderecoDTO Endereco { get; set; } = new EnderecoDTO();
        public List<DocumentoDTO> Documentos { get; set; } = new List<DocumentoDTO>();
    }
}
