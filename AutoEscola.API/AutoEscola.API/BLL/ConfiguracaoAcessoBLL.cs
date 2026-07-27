using AutoEscola.API.BLL.Interface;
using AutoEscola.API.Data;
using AutoEscola.API.Models.DTO.ControleAcesso;
using AutoEscola.API.Services;

namespace AutoEscola.API.BLL
{
    public class ConfiguracaoAcessoBLL : IConfiguracaoAcesso
    {

        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;
        private readonly IHttpContextAccessor _httpContext;

        public ConfiguracaoAcessoBLL(AppDbContext context, JwtService jwtService, IHttpContextAccessor httpContext)
        {
            _context = context;
            _jwtService = jwtService;
            _httpContext = httpContext;
        }

        public Task<ConfiguracaoAcessoDTO> Adicionar(ConfiguracaoAcessoDTO entidade)
        {
            throw new NotImplementedException();
        }

        public Task<ConfiguracaoAcessoDTO> Atualizar(ConfiguracaoAcessoDTO entidade)
        {
            throw new NotImplementedException();
        }

        public Task<ConfiguracaoAcessoDTO> BuscarPorId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ConfiguracaoAcessoDTO>> BuscarTodos(int id)
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
