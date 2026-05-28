using AutoEscola.API.BLL.Interface;
using AutoEscola.API.Data;
using AutoEscola.API.Models;
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
        public async Task<Usuario> CriarUsuario(UsuarioDTO novoUsuario)
        {

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

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            return usuario;
        }
    }
}
