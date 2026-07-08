using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AutoEscola.API.Models.Entidade;

namespace AutoEscola.API.Mappings
{
    public class InstrutorDisponivelMap : IEntityTypeConfiguration<InstrutorDisponivel>
    {
        public void Configure(EntityTypeBuilder<InstrutorDisponivel> builder)
        {
            builder.ToTable("instrutores_disponiveis");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.InstrutorId)
                .HasColumnName("instrutor_id")
                .IsRequired();

            builder.Property(x => x.DataAula)
                .HasColumnName("data_aula")
                .IsRequired();

            builder.Property(x => x.Status)
                .HasColumnName("status")
                .IsRequired();

            builder.HasOne(x => x.Instrutor)
                .WithMany(i => i.Disponibilidades)
                .HasForeignKey(x => x.InstrutorId);
        }
    }
}