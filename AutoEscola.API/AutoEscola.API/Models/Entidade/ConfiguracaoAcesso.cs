using AutoEscola.API.Enum;

namespace AutoEscola.API.Models.Entidade
{
    public class ConfiguracaoAcesso
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Rota { get; set; }
        public string Icone { get; set; }
        public int Ordem { get; set; }
        public DateTime DataAtualizacao { get; set; } = DateTime.Now;
        public int Excluido { get; set; } = (int)Status.ATIVO; 
        public ICollection<GrupoConfiguracaoAcesso> Grupos { get; set; } = new List<GrupoConfiguracaoAcesso>();
    }
}
