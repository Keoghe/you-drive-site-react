using AutoEscola.API.Models.Entidade;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoEscola.API.Mappings
{
    public class ConfiguracaoAcessoMap : IEntityTypeConfiguration<ConfiguracaoAcesso>
    {
        public void Configure(EntityTypeBuilder<ConfiguracaoAcesso> builder)
        {
            builder.ToTable("configuracao_acesso");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(c => c.Titulo)
                .HasColumnName("titulo")
                .HasMaxLength(200);

            builder.Property(c => c.Rota)
                .HasColumnName("rota")
                .HasMaxLength(500);

            builder.Property(c => c.Icone)
                .HasColumnName("icone")
                .HasMaxLength(500);

            builder.Property(c => c.Ordem)
                .HasColumnName("ordem"); 

            builder.Property(c => c.DataAtualizacao)
                .HasColumnName("data_atualizacao");

            builder.Property(c => c.Excluido)
                .HasColumnName("excluido")
                .HasDefaultValue(0);
        }
    }
}