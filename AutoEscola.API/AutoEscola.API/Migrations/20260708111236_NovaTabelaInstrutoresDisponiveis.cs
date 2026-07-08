using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoEscola.API.Migrations
{
    /// <inheritdoc />
    public partial class NovaTabelaInstrutoresDisponiveis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "instrutores_disponiveis",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    instrutor_id = table.Column<int>(type: "int", nullable: false),
                    data_aula = table.Column<DateTime>(type: "datetime2", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_instrutores_disponiveis", x => x.id);
                    table.ForeignKey(
                        name: "FK_instrutores_disponiveis_instrutores_instrutor_id",
                        column: x => x.instrutor_id,
                        principalTable: "instrutores",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_instrutores_disponiveis_instrutor_id",
                table: "instrutores_disponiveis",
                column: "instrutor_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "instrutores_disponiveis");
        }
    }
}
