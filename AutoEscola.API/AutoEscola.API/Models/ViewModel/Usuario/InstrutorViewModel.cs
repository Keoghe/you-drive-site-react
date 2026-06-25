namespace AutoEscola.API.Models.ViewModel.Usuario
{
    public class InstrutorViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Cnh { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public decimal Saldo { get; set; }
        public int TipoUsuario { get; set; }
        public int Ativo { get; set; }
    }
}
