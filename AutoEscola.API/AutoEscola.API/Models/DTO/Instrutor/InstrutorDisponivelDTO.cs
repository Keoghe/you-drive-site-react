namespace AutoEscola.API.Models.DTO.Instrutor
{
    public class InstrutorDisponivelDTO
    {
        public int Id { get; set; }
        public int InstrutorId { get; set; }
        public DateTime DataAula { get; set; }
        public int Status { get; set; }
    }
}
