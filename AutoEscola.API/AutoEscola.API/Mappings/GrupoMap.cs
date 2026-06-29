using AutoEscola.API.Models;
using AutoEscola.API.Models.Entidade;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.RegularExpressions;

namespace AutoEscola.API.Mappings
{
    public class GrupoMap : IEntityTypeConfiguration<Grupo>
    {
        public void Configure(EntityTypeBuilder<Grupo> builder)
        {
            builder.ToTable("grupos");

            builder.HasKey(g => g.Id);

            builder.Property(g => g.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(g => g.Nome)
                .HasColumnName("nome")
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(g => g.DataCriacao)
                .HasColumnName("data_criacao");

            builder.Property(g => g.UsuarioId)
                .HasColumnName("usuario_id");

            builder.Property(g => g.Excluido)
                .HasColumnName("excluido")
                .HasDefaultValue(0);

            // ✅ relacionamento com usuário
            builder.HasOne<Usuario>()
                .WithMany()
                .HasForeignKey(g => g.UsuarioId);
        }
    }
}