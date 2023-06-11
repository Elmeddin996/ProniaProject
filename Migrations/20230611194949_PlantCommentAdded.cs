using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProniaProject.Migrations
{
    public partial class PlantCommentAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlantComment_AspNetUsers_AppUserId",
                table: "PlantComment");

            migrationBuilder.DropForeignKey(
                name: "FK_PlantComment_Plants_PlantId",
                table: "PlantComment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlantComment",
                table: "PlantComment");

            migrationBuilder.RenameTable(
                name: "PlantComment",
                newName: "PlantComments");

            migrationBuilder.RenameIndex(
                name: "IX_PlantComment_PlantId",
                table: "PlantComments",
                newName: "IX_PlantComments_PlantId");

            migrationBuilder.RenameIndex(
                name: "IX_PlantComment_AppUserId",
                table: "PlantComments",
                newName: "IX_PlantComments_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlantComments",
                table: "PlantComments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlantComments_AspNetUsers_AppUserId",
                table: "PlantComments",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlantComments_Plants_PlantId",
                table: "PlantComments",
                column: "PlantId",
                principalTable: "Plants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlantComments_AspNetUsers_AppUserId",
                table: "PlantComments");

            migrationBuilder.DropForeignKey(
                name: "FK_PlantComments_Plants_PlantId",
                table: "PlantComments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlantComments",
                table: "PlantComments");

            migrationBuilder.RenameTable(
                name: "PlantComments",
                newName: "PlantComment");

            migrationBuilder.RenameIndex(
                name: "IX_PlantComments_PlantId",
                table: "PlantComment",
                newName: "IX_PlantComment_PlantId");

            migrationBuilder.RenameIndex(
                name: "IX_PlantComments_AppUserId",
                table: "PlantComment",
                newName: "IX_PlantComment_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlantComment",
                table: "PlantComment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlantComment_AspNetUsers_AppUserId",
                table: "PlantComment",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlantComment_Plants_PlantId",
                table: "PlantComment",
                column: "PlantId",
                principalTable: "Plants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
