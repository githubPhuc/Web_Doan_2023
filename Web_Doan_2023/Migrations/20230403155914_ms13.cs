using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_Doan_2023.Migrations
{
    public partial class ms13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Page_Menu_MenuId",
                table: "Page");

            migrationBuilder.DropIndex(
                name: "IX_Page_MenuId",
                table: "Page");

            migrationBuilder.DropColumn(
                name: "MenuId",
                table: "Page");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MenuId",
                table: "Page",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Page_MenuId",
                table: "Page",
                column: "MenuId");

            migrationBuilder.AddForeignKey(
                name: "FK_Page_Menu_MenuId",
                table: "Page",
                column: "MenuId",
                principalTable: "Menu",
                principalColumn: "Id");
        }
    }
}
