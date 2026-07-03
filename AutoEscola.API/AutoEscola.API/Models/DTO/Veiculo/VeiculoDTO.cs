namespace AutoEscola.API.Models.DTO.Veiculo
{
    public class VeiculoDTO
    {
        public int Id { get; set; } 
        public int InstrutorId { get; set; } 
        public string Modelo { get; set; }
        public string Cor { get; set; }
        public string Placa { get; set; }
    }
}
