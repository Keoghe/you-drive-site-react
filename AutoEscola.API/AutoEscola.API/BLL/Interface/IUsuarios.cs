using AutoEscola.API.Helper;
using AutoEscola.API.Models.DTO.Login;
using AutoEscola.API.Models.DTO.Usuario;
using AutoEscola.API.Models.ViewModel.Login;
using AutoEscola.API.Models.ViewModel.Usuario;
using AutoEscola.API.Services;

namespace AutoEscola.API.BLL.Interface
{
    public interface IUsuarios
    {
        Task<UsuarioViewModel> CriarUsuario(UsuarioDTO novoUsuario);
        Task<UsuarioViewModel> BuscarUsuarioPorId(int usuarioId);
        Task<List<UsuarioViewModel>> BuscarUsuariosPorId(List<int> usuarioId);
        Task<List<UsuarioViewModel>> BuscarUsuarios();
        Task<List<MinhaContaViewModel>> BuscarDadosMinhaConta(int usuarioId);

        Task<List<InstrutorViewModel>> BuscarInstrutores();
        Task<bool> AtualizarUsuarioPorId(List<int> usuarioId); 
        Task<LoginViewModel> ValidarLogin(LoginDTO login);

        Task<Paginacao<InstrutorViewModel>> BuscarInstrutores(int pagina = 1, int tamanhoPagina = 10);

    }
}
