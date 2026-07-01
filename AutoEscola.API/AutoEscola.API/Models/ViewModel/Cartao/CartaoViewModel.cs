namespace AutoEscola.API.Models.ViewModel.Conta
{
    public class CartaoViewModel
    {
        public int Id { get; set; }  
        public string Bandeira { get; set; }
        public string Numero { get; set; }
        public string Final { get; set; }
        public string NomeTitular { get; set; }
        public string DataVigencia { get; set; }
    }
}
