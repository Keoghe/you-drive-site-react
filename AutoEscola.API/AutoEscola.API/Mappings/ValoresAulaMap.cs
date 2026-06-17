using AutoEscola.API.Models.Entidade;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoEscola.API.Mappings;

public class ValoresAulaMap : IEntityTypeConfiguration<ValoresAula>
{
    public void Configure(EntityTypeBuilder<ValoresAula> builder)
    {
        builder.ToTable("valores_aula", "dbo");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
               .HasColumnName("id")
               .ValueGeneratedOnAdd();

        builder.Property(x => x.Descricao)
            .HasColumnName("descricao")
            .HasMaxLength(100)
            .IsUnicode(false);

        builder.Property(x => x.Valor)
            .HasColumnName("valor")
            .HasColumnType("decimal(10,2)");

        builder.Property(x => x.DuracaoMinutos)
            .HasColumnName("duracao_minutos");

        builder.Property(x => x.Excluido)
            .HasColumnName("excluido");
    }
}
