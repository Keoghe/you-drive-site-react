using AutoEscola.API.BLL.Interface;
using AutoEscola.API.Data;
using AutoEscola.API.Models.DTO.ControleAcesso;
using AutoEscola.API.Services;
using Microsoft.EntityFrameworkCore;

namespace AutoEscola.API.BLL
{
    public class ConfiguracaoAcessoBLL : IConfiguracaoAcesso
    {

        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IGrupoConfiguracaoAcesso _grupoConfiguracaoAcessoBll;

        public ConfiguracaoAcessoBLL(AppDbContext context, JwtService jwtService, IHttpContextAccessor httpContext, IGrupoConfiguracaoAcesso grupoConfiguracaoAcessoBll)
        {
            _context = context;
            _jwtService = jwtService;
            _httpContext = httpContext;
            _grupoConfiguracaoAcessoBll = grupoConfiguracaoAcessoBll;
        }
        public async Task<List<ConfiguracaoAcessoDTO>> BuscarConfigurcaoAcessoUsuario(int usuarioId)
        {

            var grupoIds = await _context.GrupoUsuario
            .Where(gu => gu.UsuarioId == usuarioId)
            .Select(gu => gu.GrupoId)
            .ToListAsync();

            if (!grupoIds.Any())
                return new List<ConfiguracaoAcessoDTO>();

            var configuracoesAcesso = new List<GrupoConfiguracaoAcessoDTO>();

            foreach (var grupoId in grupoIds)
            {
                var configuracoes = await _grupoConfiguracaoAcessoBll.BuscarConfigurcaoAcessoGrupo(grupoId);
                configuracoesAcesso.AddRange(configuracoes);
            }

            var acessosFiltrados = configuracoesAcesso.DistinctBy(ca => ca.ConfiguracaoAcessoId).ToList();

            var configuracaoUsuario = await _context.ConfiguracaoAcesso
                .Where(ca => acessosFiltrados.Select(ca => ca.ConfiguracaoAcessoId).Contains(ca.Id))
                .Select(ca => new ConfiguracaoAcessoDTO
                {
                    Id = ca.Id,
                    Titulo = ca.Titulo,
                    Rota = ca.Rota,
                    Icone = ca.Icone,
                    Ordem = ca.Ordem,
                    DataAtualizacao = ca.DataAtualizacao,
                    Excluido = ca.Excluido
                })
                .ToListAsync();


            return configuracaoUsuario;
        }
        public Task<ConfiguracaoAcessoDTO> Adicionar(ConfiguracaoAcessoDTO entidade)
        {
            throw new NotImplementedException();
        }

        public Task<ConfiguracaoAcessoDTO> Atualizar(ConfiguracaoAcessoDTO entidade)
        {
            throw new NotImplementedException();
        }

        public Task<ConfiguracaoAcessoDTO> BuscarPorId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ConfiguracaoAcessoDTO>> BuscarTodos(int id)
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
