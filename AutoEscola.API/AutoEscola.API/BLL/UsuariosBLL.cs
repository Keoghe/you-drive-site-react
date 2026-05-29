using AutoEscola.API.BLL.Interface;
using AutoEscola.API.Data;
using AutoEscola.API.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace AutoEscola.API.BLL
{
    public class UsuariosBLL : IUsuarios
    {
        private readonly AppDbContext _context;

        public UsuariosBLL(AppDbContext context)
        {
            _context = context;
        }

        public Task<bool> AtualizarUsuarioPorId(List<int> usuarioId)
        {
            throw new NotImplementedException();
        }

        public Task<Usuario> BuscarUsuarioPorId(int usuarioId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Usuario>> BuscarUsuariosPorId(List<int> usuarioId)
        {
            throw new NotImplementedException();
        }

        public async Task<Usuario> CriarUsuario(UsuarioDTO novoUsuario)
        {

            // ✅ VALIDAÇÃO DE DUPLICIDADE

            var usuarioExistente = await _context.Usuarios
                .Where(u => !u.Excluido &&
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
                Senha = novoUsuario.Senha,
                Saldo = novoUsuario.Saldo,
                DataCadastro = DateTime.Now,
                Excluido = false
            };  

            usuario.DataCadastro = DateTime.Now;
            usuario.Excluido = false;

            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();

            return usuario;
        }

        
    }
}
