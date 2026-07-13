using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoEscola.API.Migrations
{
    /// <inheritdoc />
    public partial class AjusteRelacionamentoTabelaAula : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_aulas_instrutores_InstrutorId1",
                table: "aulas");

            migrationBuilder.DropForeignKey(
                name: "FK_aulas_usuarios_UsuarioId1",
                table: "aulas");

            migrationBuilder.DropIndex(
                name: "IX_aulas_InstrutorId1",
                table: "aulas");

            migrationBuilder.DropIndex(
                name: "IX_aulas_UsuarioId1",
                table: "aulas");

            migrationBuilder.DropColumn(
                name: "InstrutorId1",
                table: "aulas");

            migrationBuilder.DropColumn(
                name: "UsuarioId1",
                table: "aulas");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InstrutorId1",
                table: "aulas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsuarioId1",
                table: "aulas",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_aulas_InstrutorId1",
                table: "aulas",
                column: "InstrutorId1");

            migrationBuilder.CreateIndex(
                name: "IX_aulas_UsuarioId1",
                table: "aulas",
                column: "UsuarioId1");

            migrationBuilder.AddForeignKey(
                name: "FK_aulas_instrutores_InstrutorId1",
                table: "aulas",
                column: "InstrutorId1",
                principalTable: "instrutores",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_aulas_usuarios_UsuarioId1",
                table: "aulas",
                column: "UsuarioId1",
                principalTable: "usuarios",
                principalColumn: "id");
        }
    }
}
