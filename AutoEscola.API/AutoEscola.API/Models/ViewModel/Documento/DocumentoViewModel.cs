namespace AutoEscola.API.Models.ViewModel.Documento
{
    public class DocumentoViewModel
    {
        public int Id { get; set; }
        public string NomeOriginal { get; set; } = string.Empty; 
        public string Base64 { get; set; } = string.Empty;
        public int Status { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.Now;
    }
}
