namespace AutoEscola.API.Models.ViewModel.Documento
{
    public class DocumentoViewModel
    {
        public int Id { get; set; }
        public string NomeOriginal { get; set; } = string.Empty; 
        public string Base64 { get; set; } = string.Empty;
        public int Status { get; set; }
        public int TipoDocumentalId { get; set; }
        public string Descricao { get; set; } = string.Empty;

        public DateTime DataCriacao { get; set; } = DateTime.Now;
    }
}
