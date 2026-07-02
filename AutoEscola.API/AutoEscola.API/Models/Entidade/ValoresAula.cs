namespace AutoEscola.API.Models.Entidade
{
    public class ValoresAula
    {
        public int Id { get; set; }

        public string? Descricao { get; set; }

        public decimal? Valor { get; set; }

        public int? DuracaoMinutos { get; set; }

        public int Excluido { get; set; }
    }
}
