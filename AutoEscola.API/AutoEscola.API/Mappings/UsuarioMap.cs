using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AutoEscola.API.Models;

namespace AutoEscola.API.Mappings
{
    public class UsuarioMap : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            // Nome da tabela
            builder.ToTable("usuarios");

            // PK
            builder.HasKey(u => u.Id);

            // Campos
            builder.Property(u => u.Nome)
                .HasColumnName("nome")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(u => u.Cpf)
                .HasColumnName("cpf")
                .HasMaxLength(14)
                .IsRequired();

            builder.Property(u => u.Cnh)
                .HasColumnName("cnh")
                .HasMaxLength(20);

            builder.Property(u => u.Email)
                .HasColumnName("email")
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(u => u.Senha)
                .HasColumnName("senha")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(u => u.Saldo)
                .HasColumnName("saldo")
                .HasColumnType("decimal(10,2)");

            builder.Property(u => u.DataNascimento)
                .HasColumnName("data_nascimento");

            builder.Property(u => u.DataCadastro)
                .HasColumnName("data_cadastro");

            builder.Property(u => u.Excluido)
                .HasColumnName("excluido")
                .HasDefaultValue(false);

            // Índices
            builder.HasIndex(u => u.Email).IsUnique();
            builder.HasIndex(u => u.Cpf).IsUnique();

            // RELACIONAMENTOS (1:N)
            //builder.HasMany(u => u.Enderecos)
            //    .WithOne(e => e.Usuario)
            //    .HasForeignKey(e => e.UsuarioId);

            //builder.HasMany(u => u.Cartoes)
            //    .WithOne(c => c.Usuario)
            //    .HasForeignKey(c => c.UsuarioId);

            //builder.HasMany(u => u.Aulas)
            //    .WithOne(a => a.Usuario)
            //    .HasForeignKey(a => a.UsuarioId);
        }
    }
}