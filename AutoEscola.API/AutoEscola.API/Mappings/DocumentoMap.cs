using AutoEscola.API.Models.Entidade;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoEscola.API.Mappings
{
    public class DocumentoMap : IEntityTypeConfiguration<Documento>
    {
        public void Configure(EntityTypeBuilder<Documento> builder)
        {
            builder.ToTable("documentos");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();


            builder.Property(c => c.NomeOriginal).HasColumnName("nome_original").HasMaxLength(100);
            builder.Property(c => c.CaminhoArquivo).HasColumnName("caminho_arquivo").HasMaxLength(500);
            builder.Property(c => c.TipoDocumentoId).HasColumnName("tipo_documento_id");
            builder.Property(c => c.Status).HasColumnName("status").HasMaxLength(100);
            builder.Property(c => c.DataCriacao).HasColumnName("data_criacao");
            builder.Property(c => c.Excluido).HasColumnName("excluido").HasDefaultValue(false);

            builder.Property(c => c.UsuarioId)
                .HasColumnName("usuario_id");


            builder.HasOne(c => c.Usuario)
                .WithMany(u => u.Documentos)
                .HasForeignKey(c => c.UsuarioId);

        }
    }
}
