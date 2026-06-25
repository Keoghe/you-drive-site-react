using AutoEscola.API.BLL.Interface;
using AutoEscola.API.Data;
using AutoEscola.API.Enum;
using AutoEscola.API.Models.DTO.Aula;
using AutoEscola.API.Models.Entidade;
using AutoEscola.API.Services;
using Microsoft.EntityFrameworkCore;

namespace AutoEscola.API.BLL
{
    public class AulasBLL : IAulas
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;
        private readonly IHttpContextAccessor _httpContext;

        public AulasBLL(AppDbContext context, JwtService jwtService, IHttpContextAccessor httpContext)
        {
            _context = context;
            _jwtService = jwtService;
            _httpContext = httpContext;
        }
        public Task<bool> AtualizarAulaPorId(AulaDTO AulaId)
        {
            throw new NotImplementedException();
        }

        public Task<Aula> BuscarAulaPorId(int AulaId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Aula>> BuscarAulas()
        {
            throw new NotImplementedException();
        }

        public Task<List<Aula>> BuscarAulasPorId(List<int> AulaId)
        {
            throw new NotImplementedException();
        }

        public async Task<Aula> CriarAula(AulaDTO novaAula)
        {
            await ValidarDadosAula(novaAula);

            var aula = new Aula
            {
                InstrutorId = novaAula.InstrutorId,
                UsuarioId = novaAula.UsuarioId,
                PromocaoId = novaAula.PromocaoId,
                DataAula = DateOnly.FromDateTime(DateTime.Now),
                HoraInicio = novaAula.HoraInicio,
                HoraFim = novaAula.HoraFim,
                ValorAulaId = novaAula.ValorAulaId,
                ValorFinal = novaAula.ValorFinal,
                Status = "ATIVO",
                Excluido = false
            };

            await _context.Aulas.AddAsync(aula);
            await _context.SaveChangesAsync();

            return aula;
        }

        private async Task ValidarDadosAula(AulaDTO novaAula)
        {
            try
            {
                //validar dados enviados antes de cadastrar
                //verificar se existe promoção ativa para aplicar o desconto no valor hora final

                var usuarioExistente = await _context.Usuarios
                    .Where(u => u.Excluido == (int)StatusContaUsuario.ATIVO && u.Id == novaAula.UsuarioId)
                    .FirstOrDefaultAsync();
                if (usuarioExistente == null)
                {
                    throw new Exception("Usuário informado não está ativo");
                }

                if (novaAula.PromocaoId != 0)
                {
                    var promocao = await _context.Promocoes
                           .Where(u => u.Excluido == (int)Status.ATIVO && u.Id == novaAula.PromocaoId)
                           .FirstOrDefaultAsync();
                    if (novaAula == null)
                    {
                        throw new Exception("Promoção informada não está ativa");
                    }
                }

                var instrutor = await _context.Instrutores
                           .Where(u => u.Excluido == (int)StatusContaUsuario.ATIVO && u.Id == novaAula.InstrutorId)
                           .FirstOrDefaultAsync();
                if (novaAula == null)
                {
                    throw new Exception("Instrutor informado não está ativo");
                } 
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
