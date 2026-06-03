using AutoEscola.API.Models.DTO.Login;
using AutoEscola.API.Models.DTO.Usuario;
using AutoEscola.API.Models.ViewModel.Login;
using AutoEscola.API.Services;

namespace AutoEscola.API.BLL.Interface
{
    public interface IUsuarios
    {
        Task<Usuario> CriarUsuario(UsuarioDTO novoUsuario);
        Task<Usuario> BuscarUsuarioPorId(int usuarioId);
        Task<List<Usuario>> BuscarUsuariosPorId(List<int> usuarioId);
        Task<List<Usuario>> BuscarUsuarios();
        Task<bool> AtualizarUsuarioPorId(List<int> usuarioId); 
        Task<LoginViewModel> ValidarLogin(LoginDTO login); 

    }
}
