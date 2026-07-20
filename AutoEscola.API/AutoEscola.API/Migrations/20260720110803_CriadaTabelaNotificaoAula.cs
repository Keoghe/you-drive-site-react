using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoEscola.API.Migrations
{
    /// <inheritdoc />
    public partial class CriadaTabelaNotificaoAula : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "notificacao_aula",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    aluno_id = table.Column<int>(type: "int", nullable: false),
                    instrutor_id = table.Column<int>(type: "int", nullable: false),
                    descricao = table.Column<string>(type: "nvarchar(600)", maxLength: 600, nullable: false),
                    data_solicitacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    excluido = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notificacao_aula", x => x.id);
                    table.ForeignKey(
                        name: "FK_notificacao_aula_usuarios_aluno_id",
                        column: x => x.aluno_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_notificacao_aula_usuarios_instrutor_id",
                        column: x => x.instrutor_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_notificacao_aula_aluno_id",
                table: "notificacao_aula",
                column: "aluno_id");

            migrationBuilder.CreateIndex(
                name: "IX_notificacao_aula_instrutor_id",
                table: "notificacao_aula",
                column: "instrutor_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "notificacao_aula");
        }
    }
}
