namespace AutoEscola.API.Models.ViewModel.Controle_Acesso
{
    public class ConfiguracaoAcessoViewModel
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Rota { get; set; }
        public string Icone { get; set; }
        public int Ordem { get; set; }
        public int GrupoId { get; set; }
        public DateTime DataAtualizacao { get; set; } = DateTime.Now;
        public int Excluido { get; set; }
    }
}
