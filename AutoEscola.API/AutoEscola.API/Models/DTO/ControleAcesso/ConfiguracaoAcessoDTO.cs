namespace AutoEscola.API.Models.DTO.ControleAcesso
{
    public class ConfiguracaoAcessoDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; } 
        public string Rota { get; set; }
        public string Icone { get; set; }
        public int Ordem { get; set; } = 0;
        public DateTime DataAtualizacao { get; set; } = DateTime.Now;
        public int Excluido { get; set; }
    }
}
