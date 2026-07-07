namespace AutoEscola.API.BLL.Interface.Base
{

    public interface IBaseBLL<T> : IDisposable where T : class
    {
        Task<T> Adicionar(T entidade);
        Task<T> Atualizar(T entidade);
        Task<bool> Remover(int id); 
        Task<T> BuscarPorId(int id);
        Task<List<T>> BuscarTodos(int id);
    }

}
