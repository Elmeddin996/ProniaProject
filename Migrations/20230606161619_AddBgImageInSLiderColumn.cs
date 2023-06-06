using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProniaProject.Migrations
{
    public partial class AddBgImageInSLiderColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BgImageName",
                table: "Sliders",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BgImageName",
                table: "Sliders");
        }
    }
}
