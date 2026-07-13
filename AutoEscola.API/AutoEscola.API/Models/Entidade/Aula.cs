namespace AutoEscola.API.Models.Entidade
{
    public class Aula
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }

        public int InstrutorId { get; set; }

        public int ValorAulaId { get; set; }

        public int PromocaoId { get; set; }

        public DateOnly? DataAula { get; set; }

        public TimeOnly? HoraInicio { get; set; }

        public TimeOnly? HoraFim { get; set; }

        public string? Status { get; set; }

        public decimal? ValorFinal { get; set; }

        public int Excluido { get; set; }

        // Navegação
        public virtual Usuario Usuario { get; set; } = null!;

        public virtual Instrutor Instrutor { get; set; } = null!;

        public virtual ValoresAula ValorAula { get; set; } = null!;

        public virtual Promocoes Promocao { get; set; } = null!;
    }
}
