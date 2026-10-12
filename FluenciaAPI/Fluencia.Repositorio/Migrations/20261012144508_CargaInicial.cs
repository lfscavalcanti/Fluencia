using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fluencia.Repositorio.Migrations
{
    public partial class CargaInicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Alunos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alunos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Questoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nivel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Topico = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Enunciado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OpcaoA = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OpcaoB = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OpcaoC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OpcaoD = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OpcaoE = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RespostaCorreta = table.Column<string>(type: "nvarchar(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questoes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Resultados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AlunoId = table.Column<int>(type: "int", nullable: false),
                    NivelIdentificado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TopicosDominados = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TopicosPendentes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlanoEstudos = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataAvaliacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resultados", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Respostas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AlunoId = table.Column<int>(type: "int", nullable: false),
                    QuestaoId = table.Column<int>(type: "int", nullable: false),
                    OpcaoEscolhida = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    NãoSei = table.Column<bool>(type: "bit", nullable: false),
                    Acertou = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Respostas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Respostas_Questoes_QuestaoId",
                        column: x => x.QuestaoId,
                        principalTable: "Questoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Respostas_QuestaoId",
                table: "Respostas",
                column: "QuestaoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alunos");

            migrationBuilder.DropTable(
                name: "Respostas");

            migrationBuilder.DropTable(
                name: "Resultados");

            migrationBuilder.DropTable(
                name: "Questoes");
        }
    }
}
