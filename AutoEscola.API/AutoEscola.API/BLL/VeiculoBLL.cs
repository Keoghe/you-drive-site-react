using AutoEscola.API.BLL.Interface;
using AutoEscola.API.Data;
using AutoEscola.API.Models.Entidade;
using AutoEscola.API.Services;

namespace AutoEscola.API.BLL
{
    public class VeiculoBLL : IVeiculo
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;
        private readonly IHttpContextAccessor _httpContext;
        public VeiculoBLL(AppDbContext context, JwtService jwtService, IHttpContextAccessor httpContext)
        {
            _context = context;
            _jwtService = jwtService;
            _httpContext = httpContext;
        }
        public Task<Veiculo> Adicionar(Veiculo entidade)
        {
            throw new NotImplementedException();
        }

        public Task<Veiculo> Atualizar(Veiculo entidade)
        {
            throw new NotImplementedException();
        }

        public Task<Veiculo> BuscarPorId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Veiculo>> BuscarTodos()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public Task<bool> Remover(int id)
        {
            throw new NotImplementedException();
        }
    }
}
