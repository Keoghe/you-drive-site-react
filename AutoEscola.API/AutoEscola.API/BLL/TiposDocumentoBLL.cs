using AutoEscola.API.BLL.Interface;
using AutoEscola.API.BLL.Interface.Base;
using AutoEscola.API.Data;
using AutoEscola.API.Enum;
using AutoEscola.API.Models.Entidade;
using AutoEscola.API.Models.ViewModel;
using AutoEscola.API.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AutoEscola.API.BLL
{
    public class TiposDocumentoBLL : ITiposDocumento
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;
        private readonly IHttpContextAccessor _httpContext;
        public TiposDocumentoBLL(AppDbContext context, JwtService jwtService, IHttpContextAccessor httpContext)
        {
            _context = context;
            _jwtService = jwtService;
            _httpContext = httpContext;
        }
        public Task<TiposDocumento> Adicionar(TiposDocumento entidade)
        {
            throw new NotImplementedException();
        }

        public Task<TiposDocumento> Atualizar(TiposDocumento entidade)
        {
            throw new NotImplementedException();
        }

        public Task<TiposDocumento> BuscarPorId(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<TiposDocumentoViewModel>> BuscarTiposDocumento(int tipoUsuarioId)
        {
            var tiposDocumentos = await BuscarTodos();

            if (tiposDocumentos == null || tiposDocumentos.Count == 0)
                throw new Exception("Nenhum tipo de documento encontrado para esse tipo de usuário.");

            var tiposDocumentosViewModel = tiposDocumentos.Where(c => c.TipoUsuarioId == tipoUsuarioId)

                .Select(td => new TiposDocumentoViewModel
                {
                    Id = td.Id,
                    Nome = td.Descricao
                }).ToList();

            if (tiposDocumentosViewModel.Count == 0)
                throw new Exception("Nenhum tipo de documento encontrado para esse tipo de usuário.");

            return tiposDocumentosViewModel;
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public Task<bool> Remover(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<TiposDocumento>> BuscarTodos()
        {
            var tiposDocumentos = await _context.TiposDocumento.Where(c =>
            c.Excluido == (int)StatusContaUsuario.ATIVO
            ).ToListAsync();

            return tiposDocumentos;
        }
    }
}
