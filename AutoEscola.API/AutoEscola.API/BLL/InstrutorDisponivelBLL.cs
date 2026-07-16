using AutoEscola.API.BLL.Interface;
using AutoEscola.API.Data;
using AutoEscola.API.Enum;
using AutoEscola.API.Models.DTO.Instrutor;
using AutoEscola.API.Models.Entidade;
using AutoEscola.API.Models.ViewModel.Instrutor;
using AutoEscola.API.Services;
using Microsoft.EntityFrameworkCore;

namespace AutoEscola.API.BLL
{
    public class InstrutorDisponivelBLL : IInstrutorDisponivel
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IInstrutor _instrutor;
        public InstrutorDisponivelBLL(AppDbContext context, JwtService jwtService, IHttpContextAccessor httpContext, IInstrutor instrutor)
        {
            _context = context;
            _jwtService = jwtService;
            _httpContext = httpContext;
            _instrutor = instrutor;
        }
        public async Task<InstrutorDisponivelDTO> Adicionar(InstrutorDisponivelDTO instrutor)
        {
            var instrutorExistente = await _context.InstrutorDisponivel
               .FirstOrDefaultAsync(i => i.InstrutorId == instrutor.InstrutorId);

            if (instrutorExistente != null && instrutorExistente.Status == (int)StatusInstrutorAula.DISPONIVEL)
                throw new Exception("Instrutor já cadastrado e ativo");

            var novoInstrutor = new InstrutorDisponivel
            {
                InstrutorId = instrutor.InstrutorId,
                DataAula = instrutor.DataAula,
                Status = (int)StatusInstrutorAula.DISPONIVEL
            };

            await _context.InstrutorDisponivel.AddAsync(novoInstrutor);
            await _context.SaveChangesAsync();

            return instrutor;
        }

        public async Task<InstrutorDisponivelDTO> Atualizar(InstrutorDisponivelDTO instrutor)
        {
            var instrutorExistente = await _context.InstrutorDisponivel
                .Include(x => x.Instrutor)
                .FirstOrDefaultAsync(x => x.Instrutor.UsuarioId == instrutor.UsuarioId);


            if (instrutorExistente == null)
            {
                var dadosIntrutor = await _instrutor.BuscarPorId(instrutor.UsuarioId);

                var novoInstrutor = await Adicionar(new InstrutorDisponivelDTO
                {
                    InstrutorId = dadosIntrutor.Id,
                    DataAula = instrutor.DataAula,
                    Status = (int)StatusInstrutorAula.DISPONIVEL
                });
            }
            else
            {
                instrutorExistente.DataAula = instrutor.DataAula;
                instrutorExistente.Status = instrutor.Status;

                await _context.SaveChangesAsync();
            }

            return instrutor;
        }

        public async Task<List<InstrutorDisponivelViewModel>> BuscarInstrutorDisponivel()
        {
            var instrutores = await _context.InstrutorDisponivel.Where(c => c.Status == (int)StatusInstrutorAula.DISPONIVEL)
                .Select(x => new InstrutorDisponivelViewModel
                {
                    Id = x.Id,
                    InstrutorId = x.InstrutorId,
                    DataAula = x.DataAula,
                    Status = x.Status
                }).ToListAsync();

            return instrutores;
        }

        public async Task<List<InstrutorDisponivelCidadeViewModel>> BuscarInstrutorDisponivelCidade(string cidade)
        {

            var instrutores = await _context
                                     .InstrutorDisponivel
                                     .Where(x =>
                                         x.Status == (int)StatusInstrutorAula.DISPONIVEL &&
                                         x.Instrutor.Cidade == cidade)
                                     .Select(x => new
                                     {
                                         Disponibilidade = x,
                                         Veiculo = x.Instrutor.Veiculos
                                             .FirstOrDefault(v => v.Excluido == (int)Status.ATIVO),
                                         Documento = x.Instrutor.Usuario.Documentos
                                             .FirstOrDefault(d => d.Excluido == (int)Status.ATIVO && d.TipoDocumentoId == (int)TipoAnexo.FOTO_SELFIE)

                                     })
                                     .Select(x => new InstrutorDisponivelCidadeViewModel
                                     {
                                         UsuarioId = x.Disponibilidade.Instrutor.UsuarioId,
                                         Nome = x.Disponibilidade.Instrutor.Usuario.Nome,

                                         Modelo = x.Veiculo.Modelo,
                                         Cor = x.Veiculo.Cor,
                                         Placa = x.Veiculo.Placa,

                                         caminhoSelfie = x.Documento.CaminhoArquivo,

                                         Status = x.Disponibilidade.Status,
                                         Avaliacao = x.Disponibilidade.Instrutor.Avaliacao,
                                         Bairro = x.Disponibilidade.Instrutor.Bairro,
                                         Cidade = x.Disponibilidade.Instrutor.Cidade,
                                         Estado = x.Disponibilidade.Instrutor.Estado,
                                         Latitude = x.Disponibilidade.Instrutor.Latitude,
                                         Longitude = x.Disponibilidade.Instrutor.Longitude,
                                         Nota = x.Disponibilidade.Instrutor.Avaliacao,
                                         Valor = x.Disponibilidade.Instrutor.ValorHora
                                     })
                                     .ToListAsync();
            instrutores.ForEach(i =>
            {
                if (!string.IsNullOrEmpty(i.caminhoSelfie) && File.Exists(i.caminhoSelfie))
                {
                    i.Foto = Convert.ToBase64String(
                        File.ReadAllBytes(i.caminhoSelfie)
                    );
                }
            });

            return instrutores;
        }

        public async Task<InstrutorDisponivelDTO> BuscarPorId(int instrutorId)
        {
            var instrutor = await _context.InstrutorDisponivel.Where(c =>
            c.Status == (int)StatusInstrutorAula.DISPONIVEL
            && c.InstrutorId == instrutorId)
                .Select(x => new InstrutorDisponivelDTO
                {
                    Id = x.Id,
                    InstrutorId = x.InstrutorId,
                    DataAula = x.DataAula,
                    Status = x.Status
                }).FirstOrDefaultAsync();

            if (instrutor == null)
                throw new Exception("Instrutor não encontrado");

            return instrutor;
        }

        public Task<List<InstrutorDisponivelDTO>> BuscarTodos(int id)
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
