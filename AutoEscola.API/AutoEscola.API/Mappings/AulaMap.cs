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

            builder.HasKey(x => x.Id); 

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.UsuarioId)
                .HasColumnName("usuario_id")
                .IsRequired();

            builder.Property(x => x.InstrutorId)
                .HasColumnName("instrutor_id")
                .IsRequired();

            builder.Property(x => x.ValorAulaId)
                .HasColumnName("valor_aula_id")
                .IsRequired();

            builder.Property(x => x.PromocaoId)
                .HasColumnName("promocao_id")
                .IsRequired();

            builder.Property(x => x.DataAula)
                .HasColumnName("data_aula");

            builder.Property(x => x.HoraInicio)
                .HasColumnName("hora_inicio");

            builder.Property(x => x.HoraFim)
                .HasColumnName("hora_fim");

            builder.Property(x => x.Status)
                .HasColumnName("status")
                .HasMaxLength(20);

            builder.Property(x => x.ValorFinal)
                .HasColumnName("valor_final")
                .HasColumnType("decimal(10,2)");

            builder.Property(x => x.Excluido)
                .HasColumnName("excluido");

            // Relacionamentos

            builder.HasOne(x => x.Usuario)
                .WithMany()
                .HasForeignKey(x => x.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Instrutor)
                .WithMany()
                .HasForeignKey(x => x.InstrutorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ValorAula)
                .WithMany()
                .HasForeignKey(x => x.ValorAulaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Promocao)
                .WithMany()
                .HasForeignKey(x => x.PromocaoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
