namespace AutoEscola.API.Models.Entidade
{
    public class Documento
    {
        public int Id { get; set; }
        public string NomeOriginal { get; set; } = string.Empty;
        public string CaminhoArquivo { get; set; } = string.Empty;
        public int TipoDocumentoId { get; set; }
        public int Status { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public int Excluido { get; set; } = 0; 
        public int UsuarioId { get; set; }
        public string DescricaoAnalise { get; set; } = string.Empty;

        public virtual Usuario? Usuario { get; set; }
    }
}
