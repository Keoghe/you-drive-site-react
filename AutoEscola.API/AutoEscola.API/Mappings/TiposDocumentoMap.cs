using AutoEscola.API.Enum;
using AutoEscola.API.Models.Entidade;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoEscola.API.Mappings
{
    public class TiposDocumentoMap : IEntityTypeConfiguration<TiposDocumento>
    {
        public void Configure(EntityTypeBuilder<TiposDocumento> builder)
        {
            builder.ToTable("tipos_documento");  

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Descricao)
                .HasColumnName("descricao")
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(x => x.TipoUsuarioId)
                .HasColumnName("tipo_usuario_id")
                .HasDefaultValue((int)Enum.TipoUsuario.Condutor);

            builder.Property(x => x.Obrigatorio)
                .HasColumnName("obrigatorio")
                .HasDefaultValue((int)SIM_NAO.NAO);

            builder.Property(x => x.Excluido)
                .HasColumnName("excluido")
                .HasDefaultValue((int)Status.ATIVO);

            builder.Property(x => x.DataCriacao)
                .HasColumnName("data_criacao")
                .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.DataAlteracao)
                .HasColumnName("data_alteracao");
        }
    }
}