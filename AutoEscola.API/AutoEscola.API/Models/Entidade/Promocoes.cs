namespace AutoEscola.API.Models.Entidade
{
    public class Promocoes
    {
        public int Id { get; set; }

        public string? Descricao { get; set; }

        public decimal? PercentualDesconto { get; set; }

        public decimal? ValorDesconto { get; set; }

        public DateTime? DataInicio { get; set; }

        public DateTime? DataFim { get; set; }

        public bool Ativa { get; set; }

        public bool Excluido { get; set; }
    }
}
