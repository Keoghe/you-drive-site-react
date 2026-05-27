using System.Text.Json.Serialization;

namespace AutoEscola.API.Models.Entidade
{
    public class Aula
    {
        public int Id { get; set; }

        // ✅ FK
        public int UsuarioId { get; set; }
        public int InstrutorId { get; set; }
        public int ValorAulaId { get; set; }

        public int? PromocaoId { get; set; }

        public DateTime DataAula { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFim { get; set; }

        public string Status { get; set; }

        public decimal ValorFinal { get; set; }

        public bool Excluido { get; set; }

        // ✅ Navegação
        [JsonIgnore]
        public Usuario Usuario { get; set; }
        public Instrutor Instrutor { get; set; }
    }
}