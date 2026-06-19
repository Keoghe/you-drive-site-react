using AutoEscola.API.Enum;
using AutoEscola.API.Models.Entidade;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoEscola.API.Mappings
{

    public class StorageMap : IEntityTypeConfiguration<Storage>
    {
        public void Configure(EntityTypeBuilder<Storage> builder)
        {
            builder.ToTable("storage");   

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Caminho)
                .HasColumnName("caminho")
                .HasMaxLength(500)  
                .IsRequired();

            builder.Property(x => x.Excluido)
                .HasColumnName("excluido")
                .HasDefaultValue((int)Status.ATIVO);
        }
    }

}
