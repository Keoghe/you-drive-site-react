using AutoEscola.API.BLL.Interface;
using AutoEscola.API.Data;
using AutoEscola.API.Models.Entidade;
using AutoEscola.API.Services;

namespace AutoEscola.API.BLL
{
    public class GrupoBLL : IGrupo
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;
        private readonly IHttpContextAccessor _httpContext;
     
        public GrupoBLL(AppDbContext context, JwtService jwtService, IHttpContextAccessor httpContext)
        {
            _context = context;
            _jwtService = jwtService;
            _httpContext = httpContext; 
        }

        public Task<Grupo> Adicionar(Grupo entidade)
        {
            throw new NotImplementedException();
        }

        public Task<Grupo> Atualizar(Grupo entidade)
        {
            throw new NotImplementedException();
        }

        public Task<Grupo> BuscarPorId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Grupo>> BuscarTodos()
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
