using AutoEscola.API.BLL.Interface;
using AutoEscola.API.Data;
using AutoEscola.API.Models.DTO.Notificacao;
using AutoEscola.API.Services;

namespace AutoEscola.API.BLL
{
    public class NotificacaoAulaBLL : INotificaoAula    
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;
        private readonly IHttpContextAccessor _httpContext;

        public NotificacaoAulaBLL(AppDbContext context, JwtService jwtService, IHttpContextAccessor httpContext)
        {
            _context = context;
            _jwtService = jwtService;
            _httpContext = httpContext;
        }

        public Task<NotificacaoAulaDTO> Adicionar(NotificacaoAulaDTO entidade)
        {
            throw new NotImplementedException();
        }

        public Task<NotificacaoAulaDTO> Atualizar(NotificacaoAulaDTO entidade)
        {
            throw new NotImplementedException();
        }

        public Task<NotificacaoAulaDTO> BuscarPorId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<NotificacaoAulaDTO>> BuscarTodos(int id)
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
