using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoEscola.API.Migrations
{
    /// <inheritdoc />
    public partial class CriadaTabelaGrupoAcessoEGrupoConfiguracaoAcesso : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "configuracao_acesso",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    titulo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    rota = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    icone = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ordem = table.Column<int>(type: "int", nullable: false),
                    data_atualizacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    excluido = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_configuracao_acesso", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "grupo_configuracao_acesso",
                columns: table => new
                {
                    grupo_id = table.Column<int>(type: "int", nullable: false),
                    configuracao_acesso_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_grupo_configuracao_acesso", x => new { x.grupo_id, x.configuracao_acesso_id });
                    table.ForeignKey(
                        name: "FK_grupo_configuracao_acesso_configuracao_acesso_configuracao_acesso_id",
                        column: x => x.configuracao_acesso_id,
                        principalTable: "configuracao_acesso",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_grupo_configuracao_acesso_grupos_grupo_id",
                        column: x => x.grupo_id,
                        principalTable: "grupos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_grupo_configuracao_acesso_configuracao_acesso_id",
                table: "grupo_configuracao_acesso",
                column: "configuracao_acesso_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "grupo_configuracao_acesso");

            migrationBuilder.DropTable(
                name: "configuracao_acesso");
        }
    }
}
