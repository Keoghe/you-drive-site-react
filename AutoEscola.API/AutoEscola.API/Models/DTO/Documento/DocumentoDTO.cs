namespace AutoEscola.API.Models.DTO.Documento
{
    public class DocumentoDTO
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string NomeOriginal { get; set; } = string.Empty;
        public string CaminhoArquivo { get; set; } = string.Empty;
        public string Base64 { get; set; } = string.Empty;
        public int TipoDocumentoId { get; set; }
        public int Status { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public string DescricaoAnalise { get; set; } = string.Empty;

    }
}
