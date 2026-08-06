using AutoEscola.API.BLL.Interface;
using AutoEscola.API.Data;
using AutoEscola.API.Enum;
using AutoEscola.API.Models.DTO.Notificacao;
using AutoEscola.API.Models.Entidade;
using AutoEscola.API.Models.ViewModel.Notific_acoes;
using AutoEscola.API.Services;
using Microsoft.EntityFrameworkCore;

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

        public async Task<NotificacaoAulaViewModel> AdicionarNovaNotificacao(NotificacaoAulaDTO novaNotificacao)
        {
            var notificacoesAtiva = await _context.NotificacaoAula
                .Where(n => n.InstrutorId == novaNotificacao.InstrutorId && n.Status == (int)StatusNotificacao.Pendente).ToListAsync();

            foreach (var notificacaoAtiva in notificacoesAtiva)
            {
                if (notificacaoAtiva.DataSolicitacao <= DateTime.Now.AddMinutes(-5))
                {
                    notificacaoAtiva.Status = (int)StatusNotificacao.Excluida;
                    _context.NotificacaoAula.Update(notificacaoAtiva);
                }
                else
                {
                    throw new Exception("Já existe uma notificação de aula pendente para este instrutor.");
                }
            }

            var notificacao = await Adicionar(novaNotificacao);

            return new NotificacaoAulaViewModel
            {
                Id = notificacao.Id,
                AlunoId = notificacao.AlunoId,
                InstrutorId = notificacao.InstrutorId,
                Descricao = notificacao.Descricao,
                DataSolicitacao = notificacao.DataSolicitacao,
                Status = notificacao.Status,
                Excluido = notificacao.Excluido
            };
        }

        public async Task<NotificacaoAulaDTO> Adicionar(NotificacaoAulaDTO notificacaoAula)
        {
            var notificacao = new NotificacaoAula
            {
                AlunoId = notificacaoAula.AlunoId,
                InstrutorId = notificacaoAula.InstrutorId,
                Descricao = notificacaoAula.Descricao,
                DataSolicitacao = DateTime.Now,
                Status = (int)StatusNotificacao.Pendente,
                Excluido = 0
            };

            await _context.NotificacaoAula.AddAsync(notificacao);
            await _context.SaveChangesAsync();

            notificacaoAula.Id = notificacao.Id;

            return notificacaoAula;
        }

        public async Task<NotificacaoAulaDTO> Atualizar(NotificacaoAulaDTO entidade)
        {
            throw new NotImplementedException();
        }

        public async Task<NotificacaoAulaDTO> BuscarPorId(int notificacaoId)
        {
            var notificacao = await _context.NotificacaoAula.Where(c => c.Id == notificacaoId).Select(x => new NotificacaoAulaDTO
            {
                Id = x.Id,
                AlunoId = x.AlunoId,
                DataSolicitacao = x.DataSolicitacao,
                Descricao = x.Descricao,
                Excluido = x.Excluido,
                InstrutorId = x.InstrutorId,
                Status = x.Status

            }).FirstOrDefaultAsync();

            if (notificacao == null) 
                throw new Exception("Notificação não encontrada.");
        
            return notificacao ?? throw new KeyNotFoundException($"Notificação {notificacaoId} não encontrada.");
        }

        public async Task<List<NotificacaoAulaDTO>> BuscarNotificaoInstrutor(int instrutorId)
        {
            var notificacao = await _context.NotificacaoAula.Where(c => c.InstrutorId == instrutorId).Select(x => new NotificacaoAulaDTO
            {
                Id = x.Id,
                AlunoId = x.AlunoId,
                DataSolicitacao = x.DataSolicitacao,
                Descricao = x.Descricao,
                Excluido = x.Excluido,
                InstrutorId = x.InstrutorId,
                Status = x.Status

            }).ToListAsync(); 

            return notificacao;
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
