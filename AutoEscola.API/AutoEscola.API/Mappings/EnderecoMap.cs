using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AutoEscola.API.Models.Entidade;

namespace AutoEscola.API.Mappings
{
    public class EnderecoMap : IEntityTypeConfiguration<Endereco>
    {
        public void Configure(EntityTypeBuilder<Endereco> builder)
        {
            builder.ToTable("enderecos");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Cep).HasColumnName("cep").HasMaxLength(10);
            builder.Property(e => e.Logradouro).HasColumnName("logradouro").HasMaxLength(150);
            builder.Property(e => e.Numero).HasColumnName("numero").HasMaxLength(20);
            builder.Property(e => e.Complemento).HasColumnName("complemento").HasMaxLength(100);
            builder.Property(e => e.Bairro).HasColumnName("bairro").HasMaxLength(100);
            builder.Property(e => e.Cidade).HasColumnName("cidade").HasMaxLength(100);
            builder.Property(e => e.Estado).HasColumnName("estado").HasMaxLength(2);

            builder.Property(e => e.Excluido).HasColumnName("excluido").HasDefaultValue(false);

            builder.HasOne(e => e.Usuario)
                .WithMany(u => u.Enderecos)
                .HasForeignKey(e => e.UsuarioId);
        }
    }
}