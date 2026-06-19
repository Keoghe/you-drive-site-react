using AutoEscola.API.Enum;

namespace AutoEscola.API.Models.Entidade
{
    public class TiposDocumento
    {
        public int Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public int TipoUsuarioId { get; set; } = (int)Enum.TipoUsuario.Condutor;  
        public int Obrigatorio { get; set; } = (int)SIM_NAO.NAO;
        public int Excluido { get; set; } = (int)Status.ATIVO;
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public DateTime DataAlteracao { get; set; }

    }
}
