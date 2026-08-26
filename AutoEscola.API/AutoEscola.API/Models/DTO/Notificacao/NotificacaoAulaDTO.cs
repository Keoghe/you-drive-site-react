namespace AutoEscola.API.Models.DTO.Notificacao
{
    public class NotificacaoAulaDTO
    {
        public int Id { get; set; }
        public int AlunoId { get; set; }
        public int InstrutorId { get; set; }
        public double LatitudeAluno { get; set; }
        public double LongitudeAluno { get; set; }
        public double LatitudeInstrutor { get; set; }
        public double LongitudeInstrutor { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public DateTime DataSolicitacao { get; set; }
        public int Status { get; set; }
        public int Excluido { get; set; }
    }
}
