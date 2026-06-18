using AutoEscola.API.Models.Entidade;

public class Usuario
{
    public int Id { get; set; }

    public string Nome { get; set; }
    public string Cpf { get; set; }
    public string Cnh { get; set; }

    public DateTime DataNascimento { get; set; }

    public string Email { get; set; }
    public string Login { get; set; }
    public string Senha { get; set; }

    public decimal Saldo { get; set; }
    public DateTime DataCadastro { get; set; }

    public int Excluido { get; set; } = 0;

    // ✅ RELACIONAMENTOS
    public List<Endereco>? Enderecos { get; set; }
    public List<Cartao>? Cartoes { get; set; }
    public List<Aula>? Aulas { get; set; }
    public Instrutor? Instrutor { get; set; } = new Instrutor();
    public List<Documento>? Documentos { get; set; } = new List<Documento>();
}