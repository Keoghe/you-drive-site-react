using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoEscola.API.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarColunasLatitudeLongitudeInstrutorTabelaNotificaoAula : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "longitude",
                table: "notificacao_aula",
                newName: "LongitudeInstrutor");

            migrationBuilder.RenameColumn(
                name: "latitude",
                table: "notificacao_aula",
                newName: "LongitudeAluno");

            migrationBuilder.AddColumn<double>(
                name: "LatitudeAluno",
                table: "notificacao_aula",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "LatitudeInstrutor",
                table: "notificacao_aula",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LatitudeAluno",
                table: "notificacao_aula");

            migrationBuilder.DropColumn(
                name: "LatitudeInstrutor",
                table: "notificacao_aula");

            migrationBuilder.RenameColumn(
                name: "LongitudeInstrutor",
                table: "notificacao_aula",
                newName: "longitude");

            migrationBuilder.RenameColumn(
                name: "LongitudeAluno",
                table: "notificacao_aula",
                newName: "latitude");
        }
    }
}
