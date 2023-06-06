using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProniaProject.Migrations
{
    public partial class PlantTableChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StockStatus",
                table: "Plants",
                newName: "Bestseller");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Bestseller",
                table: "Plants",
                newName: "StockStatus");
        }
    }
}
