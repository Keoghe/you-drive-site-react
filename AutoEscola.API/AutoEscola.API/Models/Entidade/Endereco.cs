using System.Text.Json.Serialization;

namespace AutoEscola.API.Models.Entidade
{
    public class Endereco
    {
        public int Id { get; set; }

        // ✅ FK
        public int UsuarioId { get; set; }

        public string Cep { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }

        public int Excluido { get; set; }

        // ✅ Navegação
        [JsonIgnore]
        public Usuario Usuario { get; set; }
    }
}
