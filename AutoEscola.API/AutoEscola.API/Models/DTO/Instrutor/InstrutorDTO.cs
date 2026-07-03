namespace AutoEscola.API.Models.DTO.Instrutor
{
    public class InstrutorDTO
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public decimal Avaliacao { get; set; } = 0;
        public decimal ValorHora { get; set; } = 0;
        public double Latitude { get; set; } = 0;
        public double Longitude { get; set; } = 0;
        public int Ativo { get; set; } = 0;
        public int Excluido { get; set; } = 0;
    }
}
