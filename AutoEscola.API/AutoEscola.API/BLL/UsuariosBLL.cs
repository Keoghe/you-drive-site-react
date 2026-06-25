using AutoEscola.API.BLL.Interface;
using AutoEscola.API.Data;
using AutoEscola.API.Enum;
using AutoEscola.API.Models.DTO.Login;
using AutoEscola.API.Models.DTO.Usuario;
using AutoEscola.API.Models.ViewModel.Login;
using AutoEscola.API.Models.ViewModel.Usuario;
using AutoEscola.API.Services;
using Microsoft.EntityFrameworkCore;

namespace AutoEscola.API.BLL
{
    public class UsuariosBLL : IUsuarios
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;
        private readonly IHttpContextAccessor _httpContext;

        public UsuariosBLL(AppDbContext context, JwtService jwtService, IHttpContextAccessor httpContext)
        {
            _context = context;
            _jwtService = jwtService;
            _httpContext = httpContext;
        }

        public Task<bool> AtualizarUsuarioPorId(List<int> usuarioId)
        {
            throw new NotImplementedException();
        }

        public Task<UsuarioViewModel> BuscarUsuarioPorId(int usuarioId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<UsuarioViewModel>> BuscarUsuarios()
        {

            var usuarioId = _httpContext?.HttpContext?.User
                .FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            var usuarios = await _context.Usuarios
                .Where(u => u.Excluido == (int)StatusContaUsuario.ATIVO).ToListAsync();

            var usuariosDTO = usuarios.Select(u => new UsuarioViewModel
            {
                Id = u.Id,
                Nome = u.Nome,
                Cnh = u.Cnh,
                Cpf = u.Cpf,
                DataNascimento = u.DataNascimento,
                Email = u.Email,
                Login = u.Login,
                Saldo = u.Saldo
            }).ToList();

            return usuariosDTO;
        }

        public async Task<List<InstrutorViewModel>> BuscarInstrutores()
        {

            var instrutores = await (
                from u in _context.Usuarios
                join i in _context.Instrutores
                    on u.Id equals i.UsuarioId
                where u.Excluido == (int)StatusContaUsuario.ATIVO
                && i.Excluido == (int)StatusContaUsuario.ATIVO
                select new InstrutorViewModel
                {
                    Id = u.Id,
                    Nome = u.Nome,
                    Cpf = u.Cpf,
                    Email = u.Email,
                    Ativo = i.Ativo
                }
            ).ToListAsync();

            return instrutores;
        }
        
        public Task<List<UsuarioViewModel>> BuscarUsuariosPorId(List<int> usuarioId)
        {
            throw new NotImplementedException();
        }

        public async Task<UsuarioViewModel> CriarUsuario(UsuarioDTO novoUsuario)
        {

            // ✅ VALIDAÇÃO DE DUPLICIDADE

            var usuarioExistente = await _context.Usuarios
                .Where(u =>
                           (u.Login == novoUsuario.Login ||
                            u.Cpf == novoUsuario.Cpf ||
                            u.Email == novoUsuario.Email))
                .FirstOrDefaultAsync();

            if (usuarioExistente != null)
            {
                if (usuarioExistente.Login == novoUsuario.Login)
                    throw new Exception("Já existe usuário cadastrado com esse login");

                if (usuarioExistente.Cpf == novoUsuario.Cpf)
                    throw new Exception("Já existe usuário com cadastrado esse CPF");

                if (usuarioExistente.Email == novoUsuario.Email)
                    throw new Exception("Já existe usuário com cadastrado esse e-mail");
            }


            var usuario = new Usuario
            {
                Nome = novoUsuario.Nome,
                Login = novoUsuario.Login,
                Cpf = novoUsuario.Cpf,
                Cnh = novoUsuario.Cnh,
                DataNascimento = novoUsuario.DataNascimento,
                Email = novoUsuario.Email,
                Senha = BCrypt.Net.BCrypt.HashPassword(novoUsuario.Senha),
                Saldo = novoUsuario.Saldo,
                DataCadastro = DateTime.Now,
                Excluido = (int)StatusContaUsuario.ATIVO
            };

            usuario.DataCadastro = DateTime.Now;
            usuario.Excluido = (int)StatusContaUsuario.ATIVO;
            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();

            var usuarioDTO = new UsuarioViewModel
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Cnh = usuario.Cnh,
                Cpf = usuario.Cpf,
                DataNascimento = usuario.DataNascimento,
                Email = usuario.Email,
                Login = usuario.Login,
                Saldo = usuario.Saldo
            };

            return usuarioDTO;
        }

        public async Task<LoginViewModel> ValidarLogin(LoginDTO login)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Login == login.Login);

            if (usuario == null)
                throw new Exception("Usuário não encontrado");


            bool senhaValida = BCrypt.Net.BCrypt.Verify(
                login.Senha,
                usuario.Senha
            );

            if (!senhaValida)
                throw new Exception("Senha inválida");

            var token = _jwtService.GerarToken(usuario);

            return new LoginViewModel
            {
                usuarioId = usuario.Id,
                Token = token,
                Email = usuario.Email,
                Nome = usuario.Nome
            };
        }

        
    }
}
