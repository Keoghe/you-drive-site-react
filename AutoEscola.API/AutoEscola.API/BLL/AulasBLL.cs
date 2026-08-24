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

        public Task<AulaDTO> Adicionar(AulaDTO entidade)
        {
            throw new NotImplementedException();
        }

        public Task<AulaDTO> Atualizar(AulaDTO entidade)
        {
            throw new NotImplementedException();
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

        public Task<AulaDTO> BuscarPorId(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<AulaDTO>> BuscarTodos(int usuarioId)
        {

            var usuario = await _context.Usuarios
                .Where(u => u.Id == usuarioId && u.Excluido == (int)StatusContaUsuario.ATIVO)
                .FirstOrDefaultAsync();

            if (usuario == null)
            {
                throw new Exception("Usuário não encontrado ou inativo");
            }

            var aulas = await _context.Aulas
                .Where(a => a.UsuarioId == usuarioId && a.Excluido == (int)Status.ATIVO)
                .Select(a => new AulaDTO
                {
                    Id = a.Id,
                    UsuarioId = a.UsuarioId,
                    InstrutorId = a.InstrutorId,
                    PromocaoId = a.PromocaoId,
                    DataAula = a.DataAula.HasValue ? a.DataAula.Value : null,
                    HoraInicio = a.HoraInicio.HasValue ? TimeOnly.Parse(a.HoraInicio.Value.ToString("HH:mm:ss")) : (TimeOnly?)null,
                    HoraFim = a.HoraFim.HasValue ? TimeOnly.Parse(a.HoraFim.Value.ToString("HH:mm:ss")) : (TimeOnly?)null,
                    ValorAulaId = a.ValorAulaId,
                    ValorFinal = a.ValorFinal,
                    Status = a.Status
                })
                .ToListAsync();

            return aulas;
        }

        public async Task<List<AulaDTO>> BuscarAulasMes(int usuarioId, int mes, TipoUsuario tipoUsuario)
        {

            var usuario = await _context.Usuarios
                .Where(u => u.Id == usuarioId && u.Excluido == (int)StatusContaUsuario.ATIVO)
                .FirstOrDefaultAsync();

            if (usuario == null)
            {
                throw new Exception("Usuário não encontrado ou inativo");
            }

            var aulas = await _context.Aulas
                //.Where(a => a.UsuarioId == usuarioId && a.Excluido == (int)Status.ATIVO 
                //&& a.DataAula.HasValue && a.DataAula.Value.Month == mes
                //)
                .Where(a =>
                        (
                            ((int)tipoUsuario == 1 && a.UsuarioId == usuarioId) ||
                            ((int)tipoUsuario == 2 && a.InstrutorId == usuarioId)
                        )
                        && a.Excluido == (int)Status.ATIVO
                        && a.DataAula.HasValue
                        && a.DataAula.Value.Month == mes
                    )
                .Select(a => new AulaDTO
                {
                    Id = a.Id,
                    UsuarioId = a.UsuarioId,
                    InstrutorId = a.InstrutorId,
                    PromocaoId = a.PromocaoId,
                    DataAula = a.DataAula.HasValue ? a.DataAula.Value : null,
                    HoraInicio = a.HoraInicio.HasValue ? TimeOnly.Parse(a.HoraInicio.Value.ToString("HH:mm:ss")) : (TimeOnly?)null,
                    HoraFim = a.HoraFim.HasValue ? TimeOnly.Parse(a.HoraFim.Value.ToString("HH:mm:ss")) : (TimeOnly?)null,
                    ValorAulaId = a.ValorAulaId,
                    ValorFinal = a.ValorFinal,
                    Status = a.Status
                })
                .ToListAsync();

            return aulas;
        }

        public async Task<AulaDTO> CriarAula(AulaDTO novaAula)
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
                Status = (int)StatusAula.PENDENTE,
                Excluido = (int)Status.ATIVO
            };

            await _context.Aulas.AddAsync(aula);
            await _context.SaveChangesAsync();

            novaAula.Id = aula.Id;

            return novaAula;
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public Task<bool> Remover(int id)
        {
            throw new NotImplementedException();
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
