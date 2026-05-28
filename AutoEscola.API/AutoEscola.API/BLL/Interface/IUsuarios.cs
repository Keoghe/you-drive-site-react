using AutoEscola.API.Models;

namespace AutoEscola.API.BLL.Interface
{
    public interface IUsuarios
    {
        Task<Usuario> CriarUsuario(UsuarioDTO novoUsuario);
    }
}
