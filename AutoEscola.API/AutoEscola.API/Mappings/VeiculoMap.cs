using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AutoEscola.API.Models.Entidade;

namespace AutoEscola.API.Mappings
{
    public class VeiculoMap : IEntityTypeConfiguration<Veiculo>
    {
        public void Configure(EntityTypeBuilder<Veiculo> builder)
        {
            builder.ToTable("veiculos");

            builder.HasKey(v => v.Id);
            builder.Property(x => x.Id)
               .HasColumnName("id")
               .ValueGeneratedOnAdd();

            builder.Property(v => v.Modelo).HasColumnName("modelo").HasMaxLength(100);
            builder.Property(v => v.Cor).HasColumnName("cor").HasMaxLength(50);
            builder.Property(v => v.Placa).HasColumnName("placa").HasMaxLength(10);

            builder.Property(v => v.Excluido).HasColumnName("excluido");

            builder.HasOne(v => v.Instrutor)
                .WithMany(i => i.Veiculos)
                .HasForeignKey(v => v.InstrutorId);
        }
    }
}
