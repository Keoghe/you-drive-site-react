namespace AutoEscola.API.Models.DTO
{
    public class UsuarioDTO 
    {
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Cnh { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public decimal Saldo { get; set; }
    }

}
