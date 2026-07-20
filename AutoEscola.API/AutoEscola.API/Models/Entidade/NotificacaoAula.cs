namespace AutoEscola.API.Models.Entidade
{
    public class NotificacaoAula
    {
        public int Id { get; set; }

        public int AlunoId { get; set; }

        public int InstrutorId { get; set; }

        public string Descricao { get; set; }

        public DateTime DataSolicitacao { get; set; }

        public int Status { get; set; }

        public int Excluido { get; set; }

        public virtual Usuario Aluno { get; set; }

        public virtual Usuario Instrutor { get; set; }
    }
}
