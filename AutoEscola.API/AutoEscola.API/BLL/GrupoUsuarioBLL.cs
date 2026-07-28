using AutoEscola.API.BLL.Interface;
using AutoEscola.API.Data;
using AutoEscola.API.Models.DTO.Grupo;
using AutoEscola.API.Models.Entidade;
using AutoEscola.API.Services;

namespace AutoEscola.API.BLL
{
    public class GrupoUsuarioBLL : IGrupoUsuario
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;
        private readonly IHttpContextAccessor _httpContext; 
        public GrupoUsuarioBLL(AppDbContext context, JwtService jwtService, IHttpContextAccessor httpContext)
        {
            _context = context;
            _jwtService = jwtService;
            _httpContext = httpContext; 
        }


        public async Task<GrupoUsuarioDTO> Adicionar(GrupoUsuarioDTO grupoUsuarioDTO)
        {
            var grupoUsuario = new GrupoUsuario
            {
                GrupoId = grupoUsuarioDTO.GrupoId,
                UsuarioId = grupoUsuarioDTO.UsuarioId
            };

            _context.GrupoUsuario.Add(grupoUsuario);
            await _context.SaveChangesAsync();
             
            return grupoUsuarioDTO;
        }

        public Task<GrupoUsuarioDTO> Atualizar(GrupoUsuarioDTO grupoUsuarioDTO)
        {
            throw new NotImplementedException();
        }

        public Task<GrupoUsuarioDTO> BuscarPorId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<GrupoUsuarioDTO>> BuscarTodos(int id)
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
