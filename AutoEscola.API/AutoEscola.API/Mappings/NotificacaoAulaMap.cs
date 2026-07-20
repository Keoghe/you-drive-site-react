using AutoEscola.API.Models.Entidade;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class NotificacaoAulaMap : IEntityTypeConfiguration<NotificacaoAula>
{
    public void Configure(EntityTypeBuilder<NotificacaoAula> builder)
    {
        builder.ToTable("notificacao_aula");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(c => c.AlunoId)
            .HasColumnName("aluno_id");

        builder.Property(c => c.InstrutorId)
            .HasColumnName("instrutor_id");

        builder.Property(c => c.Descricao)
            .HasColumnName("descricao")
            .HasMaxLength(600);

        builder.Property(c => c.DataSolicitacao)
            .HasColumnName("data_solicitacao");

        builder.Property(c => c.Status)
            .HasColumnName("status");

        builder.Property(c => c.Excluido)
            .HasColumnName("excluido");

        // Relacionamento Aluno
        builder.HasOne(c => c.Aluno)
            .WithMany(c => c.NotificacoesComoAluno)
            .HasForeignKey(c => c.AlunoId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relacionamento Instrutor
        builder.HasOne(c => c.Instrutor)
            .WithMany(c => c.NotificacoesComoInstrutor)
            .HasForeignKey(c => c.InstrutorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}