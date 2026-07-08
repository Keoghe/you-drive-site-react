namespace AutoEscola.API.Models.ViewModel.Instrutor
{
    public class InstrutorDisponivelViewModel
    {
        public int Id { get; set; }
        public int InstrutorId { get; set; }
        public DateTime DataAula { get; set; }
        public int Status { get; set; }
    }
}
