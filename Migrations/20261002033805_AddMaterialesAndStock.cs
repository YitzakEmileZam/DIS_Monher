using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DIS_Monher.Migrations
{
    /// <inheritdoc />
    public partial class AddMaterialesAndStock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Materiales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Codigo = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Nombre = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Descripcion = table.Column<string>(type: "TEXT", nullable: true),
                    Categoria = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    UnidadMedida = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    StockMinimo = table.Column<int>(type: "INTEGER", nullable: false),
                    StockActual = table.Column<int>(type: "INTEGER", nullable: false),
                    FechaIngreso = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UsuarioRegistro = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materiales", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MovimientosStock",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MaterialId = table.Column<int>(type: "INTEGER", nullable: false),
                    Tipo = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Cantidad = table.Column<int>(type: "INTEGER", nullable: false),
                    Motivo = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Fecha = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Responsable = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovimientosStock", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovimientosStock_Materiales_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materiales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Materiales_Codigo",
                table: "Materiales",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MovimientosStock_MaterialId",
                table: "MovimientosStock",
                column: "MaterialId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovimientosStock");

            migrationBuilder.DropTable(
                name: "Materiales");
        }
    }
}
