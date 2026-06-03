using AutoEscola.API.Models.Entidade;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace AutoEscola.API.Mappings;

public class PromocoesMap : IEntityTypeConfiguration<Promocoes>
{
    public void Configure(EntityTypeBuilder<Promocoes> builder)
    {
        builder.ToTable("promocoes", "dbo");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id");

        builder.Property(x => x.Descricao)
            .HasColumnName("descricao")
            .HasMaxLength(150)
            .IsUnicode(false);

        builder.Property(x => x.PercentualDesconto)
            .HasColumnName("percentual_desconto")
            .HasColumnType("decimal(5,2)");

        builder.Property(x => x.ValorDesconto)
            .HasColumnName("valor_desconto")
            .HasColumnType("decimal(10,2)");

        builder.Property(x => x.DataInicio)
            .HasColumnName("data_inicio");

        builder.Property(x => x.DataFim)
            .HasColumnName("data_fim");

        builder.Property(x => x.Ativa)
            .HasColumnName("ativa");

        builder.Property(x => x.Excluido)
            .HasColumnName("excluido");
    }
}
