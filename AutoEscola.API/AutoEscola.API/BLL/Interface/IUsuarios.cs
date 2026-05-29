using AutoEscola.API.Models.DTO;

namespace AutoEscola.API.BLL.Interface
{
    public interface IUsuarios
    {
        Task<Usuario> CriarUsuario(UsuarioDTO novoUsuario);
        Task<Usuario> BuscarUsuarioPorId(int usuarioId);
        Task<List<Usuario>> BuscarUsuariosPorId(List<int> usuarioId);
        Task<bool> AtualizarUsuarioPorId(List<int> usuarioId);

    }
}
