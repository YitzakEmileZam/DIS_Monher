using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DIS_Monher.Migrations
{
    /// <inheritdoc />
    public partial class RestoreMaterialCodigoIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Materiales_Codigo",
                table: "Materiales",
                column: "Codigo",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Materiales_Codigo",
                table: "Materiales");
        }
    }
}
