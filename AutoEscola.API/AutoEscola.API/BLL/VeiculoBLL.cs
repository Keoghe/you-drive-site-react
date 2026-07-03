using AutoEscola.API.BLL.Interface;
using AutoEscola.API.Data;
using AutoEscola.API.Enum;
using AutoEscola.API.Models.DTO.Instrutor;
using AutoEscola.API.Models.DTO.Veiculo;
using AutoEscola.API.Models.Entidade;
using AutoEscola.API.Models.ViewModel.Endereco;
using AutoEscola.API.Models.ViewModel.Veiculo;
using AutoEscola.API.Services;
using Microsoft.EntityFrameworkCore;

namespace AutoEscola.API.BLL
{
    public class VeiculoBLL : IVeiculo
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;
        private readonly IHttpContextAccessor _httpContext;
        public VeiculoBLL(AppDbContext context, JwtService jwtService, IHttpContextAccessor httpContext)
        {
            _context = context;
            _jwtService = jwtService;
            _httpContext = httpContext;
        }
        public Task<Veiculo> Adicionar(Veiculo entidade)
        {
            throw new NotImplementedException();
        }

        public async Task<VeiculoViewModel> AdicionarVeiculo(VeiculoDTO veiculoDTO)
        {
            var veiculo = await _context.Veiculos
                .FirstOrDefaultAsync(x => x.InstrutorId == veiculoDTO.InstrutorId);

            if (veiculo == null)
            {
                veiculo = new Veiculo();

                await _context.Veiculos.AddAsync(veiculo);
            }

            veiculo.InstrutorId = veiculoDTO.InstrutorId;
            veiculo.Modelo = veiculoDTO.Modelo;
            veiculo.Cor = veiculoDTO.Cor;
            veiculo.Placa = veiculoDTO.Placa;
            veiculo.Excluido = (int)Status.ATIVO;

            await _context.SaveChangesAsync();

            return new VeiculoViewModel
            {
                Id = veiculo.Id,
                InstrutorId = veiculo.InstrutorId,
                Modelo = veiculo.Modelo,
                Cor = veiculo.Cor,
                Placa = veiculo.Placa
            };
        }

        public async Task<VeiculoViewModel> BuscarVeiculoInstrutor(int usuarioId)
        {
            var veiculo = await _context.Usuarios
                    .Where(u => u.Id == usuarioId)
                    .SelectMany(u => u.Instrutor.Veiculos)
                    .Select(v => new VeiculoViewModel
                    {
                        Id = v.Id,
                        InstrutorId = v.InstrutorId,
                        Modelo = v.Modelo,
                        Cor = v.Cor,
                        Placa = v.Placa
                    })
                    .FirstOrDefaultAsync();

            return veiculo == null ? new VeiculoViewModel() : veiculo;
        }


        public Task<Veiculo> Atualizar(Veiculo entidade)
        {
            throw new NotImplementedException();
        }

        public Task<Veiculo> BuscarPorId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Veiculo>> BuscarTodos()
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
