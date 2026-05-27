namespace AutoEscola.API.Models.Entidade
{
    public class Veiculo
    {
        public int Id { get; set; }

        // ✅ FK
        public int InstrutorId { get; set; }

        public string Modelo { get; set; }
        public string Cor { get; set; }
        public string Placa { get; set; }

        public bool Excluido { get; set; }

        // ✅ Navegação
        public Instrutor Instrutor { get; set; }
    }
}
