using Microsoft.EntityFrameworkCore;
using AutoEscola.API.Mappings;
using AutoEscola.API.Models.Entidade;

namespace AutoEscola.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // =========================================
        // DBSETS
        // =========================================

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<Cartao> Cartoes { get; set; }
        public DbSet<Aula> Aulas { get; set; }
        public DbSet<Instrutor> Instrutores { get; set; }
        public DbSet<Veiculo> Veiculos { get; set; }
        public DbSet<ValoresAula> ValoresAula { get; set; }
        public DbSet<Promocoes> Promocoes { get; set; }
        public DbSet<Documento> Documentos { get; set; }
        public DbSet<Storage> Storage { get; set; }
        public DbSet<TiposDocumento> TiposDocumento { get; set; }

        // =========================================
        // MAPPINGS (FLUENT API)
        // =========================================

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ✅ APPLY CONFIGS
            modelBuilder.ApplyConfiguration(new UsuarioMap());

            
            modelBuilder.ApplyConfiguration(new UsuarioMap());
            modelBuilder.ApplyConfiguration(new EnderecoMap());
            modelBuilder.ApplyConfiguration(new CartaoMap());
            modelBuilder.ApplyConfiguration(new AulaMap());
            modelBuilder.ApplyConfiguration(new InstrutorMap());
            modelBuilder.ApplyConfiguration(new VeiculoMap());
            modelBuilder.ApplyConfiguration(new ValoresAulaMap());
            modelBuilder.ApplyConfiguration(new PromocoesMap());
            modelBuilder.ApplyConfiguration(new DocumentoMap());
            modelBuilder.ApplyConfiguration(new StorageMap());
            modelBuilder.ApplyConfiguration(new TiposDocumentoMap());

            base.OnModelCreating(modelBuilder);
        }
    }
}
