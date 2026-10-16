using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DIS_Monher.Migrations
{
    /// <inheritdoc />
    public partial class Agregar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Productos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Codigo = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    Nombre = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    Descripcion = table.Column<string>(type: "TEXT", maxLength: 300, nullable: true),
                    Categoria = table.Column<string>(type: "TEXT", maxLength: 60, nullable: false),
                    Acabado = table.Column<string>(type: "TEXT", maxLength: 60, nullable: true),
                    Precio = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Stock = table.Column<int>(type: "INTEGER", nullable: false),
                    Activo = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Productos_Codigo",
                table: "Productos",
                column: "Codigo",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Productos");
        }
    }
}
