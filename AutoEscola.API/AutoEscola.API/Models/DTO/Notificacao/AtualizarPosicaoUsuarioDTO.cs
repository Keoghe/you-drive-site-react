namespace AutoEscola.API.Models.DTO.Notificacao
{
    public class AtualizarPosicaoUsuarioDTO
    {
        public int NotificacaoId { get; set; }
        public double LatitudeAluno { get; set; }
        public double LongitudeAluno { get; set; }

        public double LatitudeInstrutor { get; set; }
        public double LongitudeInstrutor { get; set; }
    }
}
