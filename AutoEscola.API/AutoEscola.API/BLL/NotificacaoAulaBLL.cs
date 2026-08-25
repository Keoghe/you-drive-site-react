using AutoEscola.API.BLL.Interface;
using AutoEscola.API.Data;
using AutoEscola.API.Enum;
using AutoEscola.API.Models.DTO.Aula;
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
                LatitudeAluno = notificacaoAula.LatitudeAluno,
                LongitudeAluno = notificacaoAula.LongitudeAluno,
                LatitudeInstrutor = notificacaoAula.LatitudeInstrutor,
                LongitudeInstrutor = notificacaoAula.LongitudeInstrutor,
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

        public async Task<List<NotificacaoAulaDTO>> BuscarNotificaoInstrutor(int instrutorId, StatusNotificacaoAula statusNoticacaoAula)
        {
            var notificacao = await _context.NotificacaoAula.Where(c => c.InstrutorId == instrutorId && c.Status == (int)statusNoticacaoAula)
                .Select(x => new NotificacaoAulaDTO
                {
                    Id = x.Id,
                    AlunoId = x.AlunoId,
                    LatitudeAluno = x.LatitudeAluno,
                    LongitudeAluno = x.LongitudeAluno,
                    LatitudeInstrutor = x.LatitudeInstrutor,
                    LongitudeInstrutor = x.LongitudeInstrutor,
                    DataSolicitacao = x.DataSolicitacao,
                    Descricao = x.Descricao,
                    Excluido = x.Excluido,
                    InstrutorId = x.InstrutorId,
                    Status = x.Status

                }).ToListAsync();

            return notificacao;
        }
        public async Task<List<NotificacaoAulaDTO>> BuscarNotificaoAluno(int alunoId)
        {
            var inicioDia = DateTime.Today;

            var fimDia = inicioDia.AddDays(1);

            var notificacao = await _context.NotificacaoAula.Where(c => c.AlunoId == alunoId && 
                                                                   c.DataSolicitacao >= inicioDia && 
                                                                   c.DataSolicitacao < fimDia &&
                                                                   (c.Status == (int)StatusNotificacaoAula.PENDENTE || c.Status == (int)StatusNotificacaoAula.ACEITA)


                                                                   )
                .Select(x => new NotificacaoAulaDTO
                {
                    Id = x.Id,
                    AlunoId = x.AlunoId,
                    LatitudeAluno = x.LatitudeAluno,
                    LongitudeAluno = x.LongitudeAluno,
                    LatitudeInstrutor = x.LatitudeInstrutor,
                    LongitudeInstrutor = x.LongitudeInstrutor,
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
            //throw new NotImplementedException();
        }

        public Task<bool> Remover(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<NotificacaoAulaViewModel> AtualizarStatusNotificaoInstrutor(AlterarStatusNotificacaoAulaDTO alterarStatusNotificacaoAula)
        {
            var notificacao = _context.NotificacaoAula.FirstOrDefault(n => n.Id == alterarStatusNotificacaoAula.NotificacaoId);

            if (notificacao != null && (notificacao.Status != (int)StatusNotificacaoAula.CANCELADA || notificacao.Status != (int)StatusNotificacaoAula.RECUSADA))
            {
                notificacao.Status = alterarStatusNotificacaoAula.Status; 

                await _context.SaveChangesAsync();
            }

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

        public async Task<NotificacaoAulaViewModel> AtualizarPosicaoUsuario(AtualizarPosicaoUsuarioDTO atualizarPosicaoUsuario)
        {
            var notificacao = _context.NotificacaoAula.FirstOrDefault(n => n.Id == atualizarPosicaoUsuario.NotificacaoId);

            if (notificacao != null)
            {
                if(atualizarPosicaoUsuario.LatitudeInstrutor != 0)
                {
                    notificacao.LatitudeInstrutor = atualizarPosicaoUsuario.LatitudeInstrutor;
                }
                if(atualizarPosicaoUsuario.LongitudeInstrutor != 0)
                {
                    notificacao.LongitudeInstrutor = atualizarPosicaoUsuario.LongitudeInstrutor;
                }
                if(atualizarPosicaoUsuario.LatitudeAluno != 0)
                {
                    notificacao.LatitudeAluno = atualizarPosicaoUsuario.LatitudeAluno;
                }
                if(atualizarPosicaoUsuario.LongitudeAluno != 0)
                {
                    notificacao.LongitudeAluno = atualizarPosicaoUsuario.LongitudeAluno;
                }
                await _context.SaveChangesAsync();
            }

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
         
    }
}
