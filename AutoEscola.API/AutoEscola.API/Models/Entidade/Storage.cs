using AutoEscola.API.Enum;

namespace AutoEscola.API.Models.Entidade
{
    public class Storage
    {
        public int Id { get; set; }
        public string Caminho { get; set; } = string.Empty;
        public int Excluido { get; set; } = (int)Status.ATIVO;
    }
}
