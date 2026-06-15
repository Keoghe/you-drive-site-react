using AutoEscola.API.BLL.Interface;
using AutoEscola.API.Data;
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
        public Task<bool> AtualizarInstrutorPorId(List<int> instrutorId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Instrutor>> BuscarInstrutores()
        {
            throw new NotImplementedException();
        }

        public Task<List<Instrutor>> BuscarInstrutoresPorId(List<int> instrutorId)
        {
            throw new NotImplementedException();
        }

        public Task<Instrutor> BuscarInstrutorPorId(int instrutorId)
        {
            throw new NotImplementedException();
        }

        public async Task<Instrutor> CriarInstrutor(InstrutorDTO novoInstrutor)
        {
            // ✅ VALIDAÇÃO DE DUPLICIDADE

            var instrutorExistente = await _context.Instrutores
                .Where(u => !u.Excluido &&
                           (u.UsuarioId == novoInstrutor.UsuarioId))
                .FirstOrDefaultAsync();


            if (instrutorExistente != null)
            {
                if (instrutorExistente.UsuarioId == novoInstrutor.UsuarioId)
                    throw new Exception("Já existe usuário cadastrado com esse login"); 
            }


            var instrutor = new Instrutor
            {
                UsuarioId = novoInstrutor.UsuarioId
            };
                 
            instrutor.Excluido = false;

            await _context.Instrutores.AddAsync(instrutor);
            await _context.SaveChangesAsync();

            return instrutor;
        }
    }
}
