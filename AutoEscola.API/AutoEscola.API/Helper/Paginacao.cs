namespace AutoEscola.API.Helper
{
    public class Paginacao<T>
    {
        public List<T> Dados { get; set; }
        public int TotalRegistros { get; set; }
        public int PaginaAtual { get; set; }
        public int TotalPaginas { get; set; }
    }
}
