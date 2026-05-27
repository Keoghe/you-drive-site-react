using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AutoEscola.API.Models.Entidade;

namespace AutoEscola.API.Mappings
{
    public class CartaoMap : IEntityTypeConfiguration<Cartao>
    {
        public void Configure(EntityTypeBuilder<Cartao> builder)
        {
            builder.ToTable("cartoes");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Bandeira).HasColumnName("bandeira").HasMaxLength(50);
            builder.Property(c => c.Numero).HasColumnName("numero").HasMaxLength(20);
            builder.Property(c => c.Final).HasColumnName("final").HasMaxLength(4);
            builder.Property(c => c.NomeTitular).HasColumnName("nome_titular").HasMaxLength(100);

            builder.Property(c => c.Excluido).HasColumnName("excluido").HasDefaultValue(false);

            builder.HasOne(c => c.Usuario)
                .WithMany(u => u.Cartoes)
                .HasForeignKey(c => c.UsuarioId);
        }
    }
} 