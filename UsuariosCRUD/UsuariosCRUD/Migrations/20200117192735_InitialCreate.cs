using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UsuariosCRUD.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PrimeiroNome = table.Column<string>(nullable: false),
                    UltimoNome = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    NomeUsuario = table.Column<string>(nullable: false),
                    Senha = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrocaDeSenhas",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DataSolicitacao = table.Column<DateTime>(nullable: false),
                    SolicitacaoAtiva = table.Column<bool>(nullable: false),
                    UsuarioId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrocaDeSenhas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrocaDeSenhas_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrocaDeSenhas_UsuarioId",
                table: "TrocaDeSenhas",
                column: "UsuarioId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrocaDeSenhas");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
