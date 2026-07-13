using System.Collections.Generic;

namespace AutoEscola.API.Models.Entidade
{
    public class Instrutor
    {
        public int Id { get; set; }

        // ✅ FK para usuário
        public int UsuarioId { get; set; }

        public decimal Avaliacao { get; set; }
        public decimal ValorHora { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public int Ativo { get; set; }
        public int Excluido { get; set; }

        // ✅ RELACIONAMENTOS

        // Navegação para usuário
        public Usuario Usuario { get; set; }

        // 1 instrutor → vários veículos
        public List<Veiculo> Veiculos { get; set; } 

        public virtual ICollection<InstrutorDisponivel> Disponibilidades { get; set; }
        public virtual ICollection<Aula> AulasInstrutor { get; set; }
    }
}
