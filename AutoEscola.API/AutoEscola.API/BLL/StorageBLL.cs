using AutoEscola.API.BLL.Interface;
using AutoEscola.API.Data;
using AutoEscola.API.Enum;
using AutoEscola.API.Models.Entidade;
using AutoEscola.API.Services;
using Microsoft.EntityFrameworkCore;

namespace AutoEscola.API.BLL
{
    public class StorageBLL : IStorage
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;
        private readonly IHttpContextAccessor _httpContext;
        public StorageBLL(AppDbContext context, JwtService jwtService, IHttpContextAccessor httpContext)
        {
            _context = context;
            _jwtService = jwtService;
            _httpContext = httpContext;
        }
        public Task<Storage> Adicionar(Storage entidade)
        {
            throw new NotImplementedException();
        }

        public Task<Storage> Atualizar(Storage entidade)
        {
            throw new NotImplementedException();
        }

        public async Task<Storage> BuscarPorId(int id)
        {
            if (id == 0)
                throw new Exception("Necessário enviar o Id do usuário");

            var storage = await _context.Storage
               .Where(u => u.Excluido == (int)Status.ATIVO && u.Id == id)
               .FirstOrDefaultAsync();

            return storage;
            
        }

        public async Task<List<Storage>> BuscarTodos(int storageId)
        {
            var storage = await _context.Storage
               .Where(u => u.Excluido == (int)Status.ATIVO && u.Id == storageId).ToListAsync();

            return storage;
        }
        public async Task<List<Storage>> BuscarStoraAtivo()
        {
            var storage = await _context.Storage
               .Where(u => u.Excluido == (int)Status.ATIVO).ToListAsync();

            return storage;
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
