namespace AutoEscola.API.Models.ViewModel.Paginacao
{
    public class PaginacaoViewModel<T>
    {
        public int Pagina { get; set; }
        public int TotalRegistros { get; set; }
        public int TotalPaginas { get; set; }
        public List<T> Dados { get; set; }
    }
}
