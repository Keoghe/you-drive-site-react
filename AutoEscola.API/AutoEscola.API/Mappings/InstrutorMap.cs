using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AutoEscola.API.Models.Entidade;

namespace AutoEscola.API.Mappings
{
    public class InstrutorMap : IEntityTypeConfiguration<Instrutor>
    {
        public void Configure(EntityTypeBuilder<Instrutor> builder)
        {
            builder.ToTable("instrutores");

            builder.HasKey(i => i.Id);
            builder.Property(x => x.Id)
               .HasColumnName("id")
               .ValueGeneratedOnAdd();
            builder.Property(i => i.Avaliacao).HasColumnName("avaliacao");
            builder.Property(i => i.ValorHora).HasColumnName("valor_hora").HasColumnType("decimal(10,2)");
            builder.Property(i => i.Latitude).HasColumnName("latitude");
            builder.Property(i => i.Longitude).HasColumnName("longitude");
            builder.Property(i => i.Bairro).HasColumnName("bairro").HasMaxLength(800);
            builder.Property(i => i.Cidade).HasColumnName("cidade").HasMaxLength(800);
            builder.Property(i => i.Estado).HasColumnName("estado").HasMaxLength(10);

            builder.Property(i => i.Ativo).HasColumnName("ativo");
            builder.Property(i => i.Excluido).HasColumnName("excluido");


            builder.Property(i => i.UsuarioId)
                .HasColumnName("usuario_id"); 

            builder.HasOne(i => i.Usuario)
                .WithOne(u => u.Instrutor)
                .HasForeignKey<Instrutor>(i => i.UsuarioId);

            builder.HasMany(i => i.Veiculos)
                .WithOne(v => v.Instrutor)
                .HasForeignKey(v => v.InstrutorId);
        }
    }
}
