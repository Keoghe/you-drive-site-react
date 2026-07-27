namespace AutoEscola.API.Models.Entidade
{
    public class GrupoConfiguracaoAcesso
    { 
        public int GrupoId { get; set; }
        public Grupo Grupo { get; set; }

        public int ConfiguracaoAcessoId { get; set; }
        public ConfiguracaoAcesso ConfiguracaoAcesso { get; set; }
         
    }
}