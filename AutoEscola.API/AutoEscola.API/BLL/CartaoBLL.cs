using AutoEscola.API.BLL.Interface;
using AutoEscola.API.Data;
using AutoEscola.API.Enum;
using AutoEscola.API.Models.DTO.Cartao;
using AutoEscola.API.Models.DTO.Endereco;
using AutoEscola.API.Models.Entidade;
using AutoEscola.API.Models.ViewModel.Conta;
using AutoEscola.API.Models.ViewModel.Endereco;
using AutoEscola.API.Services;
using Microsoft.EntityFrameworkCore;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AutoEscola.API.BLL
{
    public class CartaoBLL : ICartao
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;
        private readonly IHttpContextAccessor _httpContext;
        public CartaoBLL(AppDbContext context, JwtService jwtService, IHttpContextAccessor httpContext)
        {
            _context = context;
            _jwtService = jwtService;
            _httpContext = httpContext;
        }
        public async Task<CartaoViewModel> AdicionarCartao(CartaoDTO cartaoDTO)
        {
            var cartao = await _context.Cartoes.FirstOrDefaultAsync(x => x.UsuarioId == cartaoDTO.UsuarioId);

            if (cartao == null)
            {
                cartao = new Cartao();

                await _context.Cartoes.AddAsync(cartao);
            }

            cartao.UsuarioId = cartaoDTO.UsuarioId;
            cartao.CpfCnpj = cartaoDTO.CpfCnpj;
            cartao.Numero = cartaoDTO.Numero;
            cartao.Bandeira = cartaoDTO.Bandeira;
            cartao.Codigo = cartaoDTO.Codigo;
            cartao.NomeTitular = cartaoDTO.NomeTitular;
            cartao.DataVigencia = cartaoDTO.DataVigencia;
            cartao.Excluido = (int)Status.ATIVO;

            await _context.SaveChangesAsync();

            return new CartaoViewModel
            {
                Id = cartao.Id,
                UsuarioId = cartao.UsuarioId,
                Numero = cartao.Numero,
                Bandeira = cartao.Bandeira,
                Codigo = cartao.Codigo,
                NomeTitular = cartao.NomeTitular,
                DataVigencia = cartao.DataVigencia
            };
        }

        public Task<CartaoDTO> Adicionar(CartaoDTO entidade)
        {
            throw new NotImplementedException();
        }

        public Task<CartaoDTO> Atualizar(CartaoDTO entidade)
        {
            throw new NotImplementedException();
        }

        public async Task<CartaoViewModel> BuscarCartao(int usuarioId)
        {
            var cartao = await BuscarPorId(usuarioId);

            return new CartaoViewModel
            {
                Id = cartao.Id,
                UsuarioId = cartao.UsuarioId,
                Numero = cartao.Numero,
                Bandeira = cartao.Bandeira,
                Codigo = cartao.Codigo,
                NomeTitular = cartao.NomeTitular,
                DataVigencia = cartao.DataVigencia
            };
        }

        public async Task<CartaoDTO> BuscarPorId(int usuarioId)
        {
            var cartaoDTO = new CartaoDTO();
            var cartao = await _context.Cartoes.FirstOrDefaultAsync(x => x.UsuarioId == usuarioId);
            if (cartao != null)
            {
                cartaoDTO.Id = cartao.Id  ;
                cartaoDTO.CpfCnpj = cartao.CpfCnpj;
                cartaoDTO.UsuarioId = cartao.UsuarioId;
                cartaoDTO.Numero = cartao.Numero;
                cartaoDTO.Bandeira = cartao.Bandeira;
                cartaoDTO.Codigo = cartao.Codigo;
                cartaoDTO.NomeTitular = cartao.NomeTitular;
                cartaoDTO.DataVigencia = cartao.DataVigencia;
            }

            return cartaoDTO;

        }

        public async Task<List<CartaoDTO>> BuscarTodos(int usuarioId)
        {
            var cartoes = new List<CartaoDTO>();
            var cartaoSalvos = await _context.Cartoes.Where(x => x.UsuarioId == usuarioId 
            && x.Excluido == (int)Status.ATIVO).ToListAsync();
            if (cartaoSalvos.Count > 0)
            {
                foreach (var cartao in cartaoSalvos)
                {
                    var cartaoDTO = new CartaoDTO
                    {
                        Id = cartao.Id,
                        CpfCnpj = cartao.CpfCnpj,
                        UsuarioId = cartao.UsuarioId,
                        Numero = cartao.Numero,
                        Bandeira = cartao.Bandeira,
                        Codigo = cartao.Codigo,
                        NomeTitular = cartao.NomeTitular,
                        DataVigencia = cartao.DataVigencia
                    };
                    cartoes.Add(cartaoDTO);
                }
            }    

            return cartoes;
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
