namespace AutoEscola.API.Models.Entidade
{
    public class Grupo
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; }
        public int UsuarioId { get; set; }
        public int Excluido { get; set; }

        public ICollection<GrupoConfiguracaoAcesso> Configuracoes { get; set; } = new List<GrupoConfiguracaoAcesso>();

    }
}
