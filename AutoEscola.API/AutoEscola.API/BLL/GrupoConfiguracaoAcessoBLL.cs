using AutoEscola.API.BLL.Interface;
using AutoEscola.API.BLL.Interface.Base;
using AutoEscola.API.Data;
using AutoEscola.API.Models.DTO.ControleAcesso;
using AutoEscola.API.Models.Entidade;
using AutoEscola.API.Models.ViewModel.Usuario;
using AutoEscola.API.Services;
using Microsoft.EntityFrameworkCore;

namespace AutoEscola.API.BLL
{
    public class GrupoConfiguracaoAcessoBLL : IGrupoConfiguracaoAcesso
    {

        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;
        private readonly IHttpContextAccessor _httpContext;

        public GrupoConfiguracaoAcessoBLL(AppDbContext context, JwtService jwtService, IHttpContextAccessor httpContext)
        {
            _context = context;
            _jwtService = jwtService;
            _httpContext = httpContext;
        }

        public async Task<GrupoConfiguracaoAcessoDTO> Adicionar(GrupoConfiguracaoAcessoDTO entidade)
        {
            var grupoConfiguracaoAcesso = new GrupoConfiguracaoAcesso
            {
                GrupoId = entidade.GrupoId,
                ConfiguracaoAcessoId = entidade.ConfiguracaoAcessoId
            };
            await _context.GrupoConfiguracaoAcesso.AddAsync(grupoConfiguracaoAcesso);
            await _context.SaveChangesAsync();

            return new GrupoConfiguracaoAcessoDTO
            {
                GrupoId = grupoConfiguracaoAcesso.GrupoId,
                ConfiguracaoAcessoId = grupoConfiguracaoAcesso.ConfiguracaoAcessoId
            };
        }

        public Task<GrupoConfiguracaoAcessoDTO> Atualizar(GrupoConfiguracaoAcessoDTO entidade)
        {
            throw new NotImplementedException();
        }

        public async Task<GrupoConfiguracaoAcessoDTO> BuscarPorId(int usuarioId)
        {
            var acessos = await _context.GrupoConfiguracaoAcesso
                .Where(g => g.GrupoId == usuarioId)
                .Select(g => new GrupoConfiguracaoAcessoDTO
                {
                    GrupoId = g.GrupoId,
                    ConfiguracaoAcessoId = g.ConfiguracaoAcessoId
                })
                .ToListAsync();
            if (acessos == null || !acessos.Any())
            {
                acessos = new List<GrupoConfiguracaoAcessoDTO>();
            }

            return acessos?.FirstOrDefault();
        }

        public async Task<List<GrupoConfiguracaoAcessoDTO>> BuscarConfigurcaoAcessoGrupo(int grupoId)
        {  
            var acessos = await _context.GrupoConfiguracaoAcesso
                .Where(gca => gca.GrupoId == grupoId)
                .Select(gca => new GrupoConfiguracaoAcessoDTO
                {
                    GrupoId = gca.GrupoId,
                    ConfiguracaoAcessoId = gca.ConfiguracaoAcessoId
                })
                .ToListAsync();

            return acessos;
        }

        public async Task<List<GrupoConfiguracaoAcessoDTO>> BuscarTodos(int usuarioId)
        {
            var teste = await _context.GrupoUsuario
                .Where(c => c.UsuarioId == usuarioId)
                .ToListAsync();


            var acessos = await _context.GrupoConfiguracaoAcesso
                .Where(g => g.GrupoId == usuarioId)
                .Select(g => new GrupoConfiguracaoAcessoDTO
                {
                    GrupoId = g.GrupoId,
                    ConfiguracaoAcessoId = g.ConfiguracaoAcessoId
                })
                .ToListAsync();

            if (acessos == null || !acessos.Any())
            {
                acessos = new List<GrupoConfiguracaoAcessoDTO>();
            }

            return acessos;
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
