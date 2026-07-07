using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoEscola.API.Migrations
{
    /// <inheritdoc />
    public partial class AdicionadaColunaCpfCnpjTabelaCartoes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "cpf_cnpj",
                table: "cartoes",
                type: "nvarchar(14)",
                maxLength: 14,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cpf_cnpj",
                table: "cartoes");
        }
    }
}
