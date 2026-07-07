using AutoEscola.API.BLL.Interface;
using AutoEscola.API.Data;
using AutoEscola.API.Models.Entidade;
using AutoEscola.API.Services;

namespace AutoEscola.API.BLL
{
    public class GrupoUsuarioBLL : IGrupoUsuario
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;
        private readonly IHttpContextAccessor _httpContext; 
        public GrupoUsuarioBLL(AppDbContext context, JwtService jwtService, IHttpContextAccessor httpContext)
        {
            _context = context;
            _jwtService = jwtService;
            _httpContext = httpContext; 
        }
        public Task<GrupoUsuario> Adicionar(GrupoUsuario entidade)
        {
            throw new NotImplementedException();
        }

        public Task<GrupoUsuario> Atualizar(GrupoUsuario entidade)
        {
            throw new NotImplementedException();
        }

        public Task<GrupoUsuario> BuscarPorId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<GrupoUsuario>> BuscarTodos(int id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Remover(int id)
        {
            throw new NotImplementedException();
        }
    }
}
