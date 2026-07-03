using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoEscola.API.Migrations
{
    /// <inheritdoc />
    public partial class AlterarNomeColunaTabelaVeiculo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_veiculos_instrutores_InstrutorId",
                table: "veiculos");

            migrationBuilder.RenameColumn(
                name: "InstrutorId",
                table: "veiculos",
                newName: "instrutor_id");

            migrationBuilder.RenameIndex(
                name: "IX_veiculos_InstrutorId",
                table: "veiculos",
                newName: "IX_veiculos_instrutor_id");

            migrationBuilder.AddForeignKey(
                name: "FK_veiculos_instrutores_instrutor_id",
                table: "veiculos",
                column: "instrutor_id",
                principalTable: "instrutores",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_veiculos_instrutores_instrutor_id",
                table: "veiculos");

            migrationBuilder.RenameColumn(
                name: "instrutor_id",
                table: "veiculos",
                newName: "InstrutorId");

            migrationBuilder.RenameIndex(
                name: "IX_veiculos_instrutor_id",
                table: "veiculos",
                newName: "IX_veiculos_InstrutorId");

            migrationBuilder.AddForeignKey(
                name: "FK_veiculos_instrutores_InstrutorId",
                table: "veiculos",
                column: "InstrutorId",
                principalTable: "instrutores",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
