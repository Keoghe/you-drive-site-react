using AutoEscola.API.BLL.Interface;
using AutoEscola.API.Data;
using AutoEscola.API.Enum;
using AutoEscola.API.Models.DTO.Endereco;
using AutoEscola.API.Models.Entidade;
using AutoEscola.API.Models.ViewModel.Endereco;
using AutoEscola.API.Models.ViewModel.Usuario;
using AutoEscola.API.Services;
using Microsoft.EntityFrameworkCore;

namespace AutoEscola.API.BLL
{
    public class EnderecoBLL : IEndereco
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;
        private readonly IHttpContextAccessor _httpContext;
        public EnderecoBLL(AppDbContext context, JwtService jwtService, IHttpContextAccessor httpContext)
        {
            _context = context;
            _jwtService = jwtService;
            _httpContext = httpContext;
        }
        public Task<EnderecoDTO> Adicionar(EnderecoDTO entidade)
        {
            throw new NotImplementedException();
        }

        public async Task<EnderecoViewModel> AdicionarEndereco(EnderecoDTO enderecoDTO)
        {
            var endereco = await _context.Enderecos
                .FirstOrDefaultAsync(x => x.UsuarioId == enderecoDTO.UsuarioId);

            if (endereco == null)
            {
                endereco = new Endereco();

                await _context.Enderecos.AddAsync(endereco);
            }

            endereco.UsuarioId = enderecoDTO.UsuarioId;
            endereco.Logradouro = enderecoDTO.Logradouro;
            endereco.Numero = enderecoDTO.Numero;
            endereco.Complemento = enderecoDTO.Complemento;
            endereco.Bairro = enderecoDTO.Bairro;
            endereco.Cep = enderecoDTO.Cep;
            endereco.Cidade = enderecoDTO.Cidade;
            endereco.Estado = enderecoDTO.Estado;
            endereco.Excluido = (int)Status.ATIVO;

            await _context.SaveChangesAsync();

            return new EnderecoViewModel
            {
                Id = endereco.Id,
                UsuarioId = endereco.UsuarioId,
                Logradouro = endereco.Logradouro,
                Numero = endereco.Numero,
                Complemento = endereco.Complemento,
                Bairro = endereco.Bairro,
                Cep = endereco.Cep,
                Cidade = endereco.Cidade,
                Estado = endereco.Estado
            };
        }

        public async Task<EnderecoViewModel> BuscarEndereco(int usuarioId)
        {
            var endereco = await BuscarPorId(usuarioId);
            
            return new EnderecoViewModel
            {
                Id = endereco.Id,
                UsuarioId = endereco.UsuarioId,
                Logradouro = endereco.Logradouro,
                Numero = endereco.Numero,
                Complemento = endereco.Complemento,
                Bairro = endereco.Bairro,
                Cep = endereco.Cep,
                Cidade = endereco.Cidade,
                Estado = endereco.Estado
            };
        }
        public Task<EnderecoDTO> Atualizar(EnderecoDTO enderecoDTO)
        {
            throw new NotImplementedException();
        }

        public async Task<EnderecoDTO> BuscarPorId(int usuarioId)
        {
            var enderecoDTO = new EnderecoDTO();
            var endereco = await _context.Enderecos
                .FirstOrDefaultAsync(x => x.UsuarioId == usuarioId);

            if (endereco != null)
            {
                enderecoDTO.Id = endereco.Id;
                enderecoDTO.UsuarioId = endereco.UsuarioId;
                enderecoDTO.Logradouro = endereco.Logradouro;
                enderecoDTO.Numero = endereco.Numero;
                enderecoDTO.Complemento = endereco.Complemento;
                enderecoDTO.Bairro = endereco.Bairro;
                enderecoDTO.Cep = endereco.Cep;
                enderecoDTO.Cidade = endereco.Cidade;
                enderecoDTO.Estado = endereco.Estado;
            }

            return enderecoDTO;
        }

        public Task<List<EnderecoDTO>> BuscarTodos(int usuarioId)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            // throw new NotImplementedException();
        }

        public Task<bool> Remover(int id)
        {
            throw new NotImplementedException();
        }
    }
}
