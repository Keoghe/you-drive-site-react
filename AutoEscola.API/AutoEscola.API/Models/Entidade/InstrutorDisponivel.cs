namespace AutoEscola.API.Models.Entidade
{
    public class InstrutorDisponivel
    {
        public int Id { get; set; }
        public int InstrutorId { get; set; }
        public DateTime DataAula { get; set; }
        public int Status { get; set; }

        public virtual Instrutor Instrutor { get; set; }
    }
}
