namespace AutoEscola.API.Models.DTO.Aula
{
    public class AulaDTO
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }

        public int InstrutorId { get; set; }

        public int ValorAulaId { get; set; }

        public int PromocaoId { get; set; }

        public DateOnly? DataAula { get; set; }

        public TimeOnly? HoraInicio { get; set; }

        public TimeOnly? HoraFim { get; set; }

        public int Status { get; set; }

        public decimal? ValorFinal { get; set; }

        public int Excluido { get; set; }
    }
}
