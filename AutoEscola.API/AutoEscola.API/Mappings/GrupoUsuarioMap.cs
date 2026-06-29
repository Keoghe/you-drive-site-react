using AutoEscola.API.Models;
using AutoEscola.API.Models.Entidade;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoEscola.API.Mappings
{
    public class GrupoUsuarioMap : IEntityTypeConfiguration<GrupoUsuario>
    {
        public void Configure(EntityTypeBuilder<GrupoUsuario> builder)
        {
            builder.ToTable("grupo_usuario");

            // ✅ chave composta (SEM ID)
            builder.HasKey(gu => new { gu.GrupoId, gu.UsuarioId });

            builder.Property(gu => gu.GrupoId)
                .HasColumnName("grupo_id");

            builder.Property(gu => gu.UsuarioId)
                .HasColumnName("usuario_id");

            // ✅ relacionamento com Grupo
            builder.HasOne<Grupo>()
                .WithMany()
                .HasForeignKey(gu => gu.GrupoId).OnDelete(DeleteBehavior.Restrict);

            // ✅ relacionamento com Usuario
            builder.HasOne<Usuario>()
                .WithMany()
                .HasForeignKey(gu => gu.UsuarioId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}