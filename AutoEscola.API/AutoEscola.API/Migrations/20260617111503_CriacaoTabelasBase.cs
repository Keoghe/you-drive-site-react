using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoEscola.API.Migrations
{
    /// <inheritdoc />
    public partial class CriacaoTabelasBase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "promocoes",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    descricao = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: true),
                    percentual_desconto = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    valor_desconto = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    data_inicio = table.Column<DateTime>(type: "datetime2", nullable: true),
                    data_fim = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ativa = table.Column<bool>(type: "bit", nullable: false),
                    excluido = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_promocoes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "usuarios",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    cpf = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false),
                    cnh = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    data_nascimento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Login = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    senha = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    saldo = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    data_cadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    excluido = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuarios", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "valores_aula",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    descricao = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    valor = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    duracao_minutos = table.Column<int>(type: "int", nullable: true),
                    excluido = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_valores_aula", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "cartoes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    bandeira = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    numero = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    final = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    nome_titular = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    excluido = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cartoes", x => x.id);
                    table.ForeignKey(
                        name: "FK_cartoes_usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "documentos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome_original = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    caminho_arquivo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    tipo_documento_id = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<int>(type: "int", maxLength: 100, nullable: false),
                    data_criacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    excluido = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    usuario_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_documentos", x => x.id);
                    table.ForeignKey(
                        name: "FK_documentos_usuarios_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "enderecos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    cep = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    logradouro = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    numero = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    complemento = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    bairro = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    cidade = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    estado = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    excluido = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_enderecos", x => x.id);
                    table.ForeignKey(
                        name: "FK_enderecos_usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "instrutores",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    usuario_id = table.Column<int>(type: "int", nullable: false),
                    avaliacao = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    valor_hora = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    latitude = table.Column<double>(type: "float", nullable: false),
                    longitude = table.Column<double>(type: "float", nullable: false),
                    ativo = table.Column<bool>(type: "bit", nullable: false),
                    excluido = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_instrutores", x => x.id);
                    table.ForeignKey(
                        name: "FK_instrutores_usuarios_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "aulas",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    usuario_id = table.Column<int>(type: "int", nullable: false),
                    instrutor_id = table.Column<int>(type: "int", nullable: false),
                    valor_aula_id = table.Column<int>(type: "int", nullable: false),
                    promocao_id = table.Column<int>(type: "int", nullable: false),
                    data_aula = table.Column<DateOnly>(type: "date", nullable: true),
                    hora_inicio = table.Column<TimeOnly>(type: "time", nullable: true),
                    hora_fim = table.Column<TimeOnly>(type: "time", nullable: true),
                    status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    valor_final = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    excluido = table.Column<bool>(type: "bit", nullable: false),
                    InstrutorId1 = table.Column<int>(type: "int", nullable: false),
                    UsuarioId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aulas", x => x.id);
                    table.ForeignKey(
                        name: "FK_aulas_instrutores_InstrutorId1",
                        column: x => x.InstrutorId1,
                        principalTable: "instrutores",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_aulas_promocoes_promocao_id",
                        column: x => x.promocao_id,
                        principalSchema: "dbo",
                        principalTable: "promocoes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_aulas_usuarios_UsuarioId1",
                        column: x => x.UsuarioId1,
                        principalTable: "usuarios",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_aulas_usuarios_instrutor_id",
                        column: x => x.instrutor_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_aulas_usuarios_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_aulas_valores_aula_valor_aula_id",
                        column: x => x.valor_aula_id,
                        principalSchema: "dbo",
                        principalTable: "valores_aula",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "veiculos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InstrutorId = table.Column<int>(type: "int", nullable: false),
                    modelo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    cor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    placa = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    excluido = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_veiculos", x => x.id);
                    table.ForeignKey(
                        name: "FK_veiculos_instrutores_InstrutorId",
                        column: x => x.InstrutorId,
                        principalTable: "instrutores",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_aulas_instrutor_id",
                table: "aulas",
                column: "instrutor_id");

            migrationBuilder.CreateIndex(
                name: "IX_aulas_InstrutorId1",
                table: "aulas",
                column: "InstrutorId1");

            migrationBuilder.CreateIndex(
                name: "IX_aulas_promocao_id",
                table: "aulas",
                column: "promocao_id");

            migrationBuilder.CreateIndex(
                name: "IX_aulas_usuario_id",
                table: "aulas",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_aulas_UsuarioId1",
                table: "aulas",
                column: "UsuarioId1");

            migrationBuilder.CreateIndex(
                name: "IX_aulas_valor_aula_id",
                table: "aulas",
                column: "valor_aula_id");

            migrationBuilder.CreateIndex(
                name: "IX_cartoes_UsuarioId",
                table: "cartoes",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_documentos_usuario_id",
                table: "documentos",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_enderecos_UsuarioId",
                table: "enderecos",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_instrutores_usuario_id",
                table: "instrutores",
                column: "usuario_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_usuarios_cpf",
                table: "usuarios",
                column: "cpf",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_usuarios_email",
                table: "usuarios",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_veiculos_InstrutorId",
                table: "veiculos",
                column: "InstrutorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "aulas");

            migrationBuilder.DropTable(
                name: "cartoes");

            migrationBuilder.DropTable(
                name: "documentos");

            migrationBuilder.DropTable(
                name: "enderecos");

            migrationBuilder.DropTable(
                name: "veiculos");

            migrationBuilder.DropTable(
                name: "promocoes",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "valores_aula",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "instrutores");

            migrationBuilder.DropTable(
                name: "usuarios");
        }
    }
}
