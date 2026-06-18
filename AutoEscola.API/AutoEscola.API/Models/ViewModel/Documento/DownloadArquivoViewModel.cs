namespace AutoEscola.API.Models.ViewModel.Documento
{
    public class DownloadArquivoViewModel
    {
        public int Id { get; set; }
        public string NomeOriginal { get; set; } = string.Empty;
        public int Status { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public string ArquivoBase64 { get; set; } = string.Empty;
    }
}
