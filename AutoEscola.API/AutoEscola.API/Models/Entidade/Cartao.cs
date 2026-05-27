using System.Text.Json.Serialization;

namespace AutoEscola.API.Models.Entidade
{
    public class Cartao
    {
        public int Id { get; set; }

        // ✅ FK
        public int UsuarioId { get; set; }

        public string Bandeira { get; set; }
        public string Numero { get; set; }
        public string Final { get; set; }
        public string NomeTitular { get; set; }

        public bool Excluido { get; set; }

        // ✅ Navegação
        [JsonIgnore]
        public Usuario Usuario { get; set; }
    }
}