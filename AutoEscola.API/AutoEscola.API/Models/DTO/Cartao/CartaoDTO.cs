namespace AutoEscola.API.Models.DTO.Cartao
{
    public class CartaoDTO
    {
        public int Id { get; set; } 
        public int UsuarioId { get; set; } 
        public string CpfCnpj { get; set; }
        public string Bandeira { get; set; }
        public string Numero { get; set; }
        public string Codigo { get; set; }
        public string NomeTitular { get; set; }
        public string DataVigencia { get; set; } 
        public int Excluido { get; set; }
    }
}
