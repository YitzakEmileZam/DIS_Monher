using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DIS_Monher.Migrations
{
    /// <inheritdoc />
    public partial class CrearModuloDespacho : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Despachos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NumeroPedido = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    DireccionDestino = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                    FechaProgramada = table.Column<DateTime>(type: "TEXT", nullable: false),
                    RangoHorario = table.Column<string>(type: "TEXT", nullable: false),
                    ReferenciasAdicionales = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Vehiculo = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Conductor = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Estado = table.Column<string>(type: "TEXT", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Despachos", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Despachos");
        }
    }
}
