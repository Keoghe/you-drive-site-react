using AutoEscola.API.BLL.Interface;
using AutoEscola.API.Data;
using AutoEscola.API.Services;

namespace AutoEscola.API.BLL
{
    public class GrupoConfiguracaoAcessoBLL : IGrupoConfiguracaoAcesso
    {

        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;
        private readonly IHttpContextAccessor _httpContext;

        public GrupoConfiguracaoAcessoBLL(AppDbContext context, JwtService jwtService, IHttpContextAccessor httpContext)
        {
            _context = context;
            _jwtService = jwtService;
            _httpContext = httpContext;
        }

        public Task<IGrupoConfiguracaoAcesso> Adicionar(IGrupoConfiguracaoAcesso entidade)
        {
            throw new NotImplementedException();
        }

        public Task<IGrupoConfiguracaoAcesso> Atualizar(IGrupoConfiguracaoAcesso entidade)
        {
            throw new NotImplementedException();
        }

        public Task<IGrupoConfiguracaoAcesso> BuscarPorId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<IGrupoConfiguracaoAcesso>> BuscarTodos(int id)
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
