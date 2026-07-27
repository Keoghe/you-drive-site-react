using AutoEscola.API.Models.Entidade;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoEscola.API.Mappings
{
    public class GrupoConfiguracaoAcessoMap : IEntityTypeConfiguration<GrupoConfiguracaoAcesso>
    {
        public void Configure(EntityTypeBuilder<GrupoConfiguracaoAcesso> builder)
        {
            builder.ToTable("grupo_configuracao_acesso");

            builder.HasKey(x => new
            {
                x.GrupoId,
                x.ConfiguracaoAcessoId
            }); 

            builder.Property(x => x.GrupoId)
                .HasColumnName("grupo_id");

            builder.Property(x => x.ConfiguracaoAcessoId)
                .HasColumnName("configuracao_acesso_id");

            builder.HasOne(x => x.Grupo)
                .WithMany(x => x.Configuracoes)
                .HasForeignKey(x => x.GrupoId);

            builder.HasOne(x => x.ConfiguracaoAcesso)
                .WithMany(x => x.Grupos)
                .HasForeignKey(x => x.ConfiguracaoAcessoId);
        }
    }
}