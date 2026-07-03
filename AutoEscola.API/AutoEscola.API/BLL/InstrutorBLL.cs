using AutoEscola.API.BLL.Interface;
using AutoEscola.API.Data;
using AutoEscola.API.Enum;
using AutoEscola.API.Models.DTO.Instrutor;
using AutoEscola.API.Models.Entidade;
using AutoEscola.API.Services;
using Microsoft.EntityFrameworkCore;

namespace AutoEscola.API.BLL
{
    public class InstrutorBLL : IInstrutor
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;
        private readonly IHttpContextAccessor _httpContext;
        public InstrutorBLL(AppDbContext context, JwtService jwtService, IHttpContextAccessor httpContext)
        {
            _context = context;
            _jwtService = jwtService;
            _httpContext = httpContext;
        }

        public async Task<InstrutorDTO> Adicionar(InstrutorDTO instrutor)

        {
            var instrutorExistente = await _context.Instrutores
                .FirstOrDefaultAsync(i => i.UsuarioId == instrutor.UsuarioId);

            if (instrutorExistente != null && instrutorExistente.Ativo == 1)
                throw new Exception("Instrutor já cadastrado e ativo");

            var novoInstrutor = new Instrutor
            {
                UsuarioId = instrutor.UsuarioId,
                Avaliacao = instrutor.Avaliacao,
                ValorHora = instrutor.ValorHora,
                Latitude = instrutor.Latitude,
                Longitude = instrutor.Longitude,
                Ativo = (int)StatusContaUsuario.ATIVO,
                Excluido = 0
            };

            await _context.Instrutores.AddAsync(novoInstrutor);
            await _context.SaveChangesAsync();

            return instrutor;
        }


        public async Task<InstrutorDTO> Atualizar(InstrutorDTO instrutor)
        {

            var instrutorExistente = await _context.Instrutores
                   .FirstOrDefaultAsync(i => i.UsuarioId == instrutor.UsuarioId);

            if (instrutorExistente == null)
                throw new Exception("Instrutor não encontrado");

            instrutorExistente.Avaliacao = instrutor.Avaliacao;
            instrutorExistente.ValorHora = instrutor.ValorHora;
            instrutorExistente.Latitude = instrutor.Latitude;
            instrutorExistente.Longitude = instrutor.Longitude;
            instrutorExistente.Ativo = instrutor.Ativo;
            instrutorExistente.Excluido = instrutor.Excluido;

            await _context.SaveChangesAsync();

            return instrutor;
        }

        public async Task<InstrutorDTO> BuscarPorId(int id)
        { 
            var instrutor = await _context.Instrutores
                            .AsNoTracking()
                           .Where(i => i.Id == id)
                           .Select(i => new InstrutorDTO
                           {
                               Id = i.Id,
                               UsuarioId = i.UsuarioId,
                               Avaliacao = i.Avaliacao,
                               ValorHora = i.ValorHora,
                               Latitude = i.Latitude,
                               Longitude = i.Longitude,
                               Ativo = i.Ativo,
                               Excluido = i.Excluido
                           })
                           .FirstOrDefaultAsync();  

            return instrutor == null ? new InstrutorDTO() : instrutor;

        }

        public async Task<List<InstrutorDTO>> BuscarTodos()
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
