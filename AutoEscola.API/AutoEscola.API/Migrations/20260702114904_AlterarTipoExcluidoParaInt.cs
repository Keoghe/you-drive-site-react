using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoEscola.API.Migrations
{
    /// <inheritdoc />
    public partial class AlterarTipoExcluidoParaInt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "excluido",
                table: "veiculos",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<int>(
                name: "excluido",
                schema: "dbo",
                table: "valores_aula",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<int>(
                name: "excluido",
                table: "enderecos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "excluido",
                table: "cartoes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "excluido",
                table: "aulas",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "excluido",
                table: "veiculos",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "excluido",
                schema: "dbo",
                table: "valores_aula",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "excluido",
                table: "enderecos",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<bool>(
                name: "excluido",
                table: "cartoes",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<bool>(
                name: "excluido",
                table: "aulas",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
