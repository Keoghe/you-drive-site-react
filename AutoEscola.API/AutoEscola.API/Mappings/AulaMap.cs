using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AutoEscola.API.Models.Entidade;

namespace AutoEscola.API.Mappings
{
    public class AulaMap : IEntityTypeConfiguration<Aula>
    {
        public void Configure(EntityTypeBuilder<Aula> builder)
        {
            builder.ToTable("aulas");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.DataAula).HasColumnName("data_aula");
            builder.Property(a => a.HoraInicio).HasColumnName("hora_inicio");
            builder.Property(a => a.HoraFim).HasColumnName("hora_fim");
            builder.Property(a => a.Status).HasColumnName("status").HasMaxLength(20);
            builder.Property(a => a.ValorFinal).HasColumnName("valor_final").HasColumnType("decimal(10,2)");

            builder.Property(a => a.Excluido).HasColumnName("excluido").HasDefaultValue(false);

            builder.HasOne(a => a.Usuario)
                .WithMany(u => u.Aulas)
                .HasForeignKey(a => a.UsuarioId);

            builder.HasOne(a => a.Instrutor)
                .WithMany(i => i.Aulas)
                .HasForeignKey(a => a.InstrutorId);
        }
    }
}
