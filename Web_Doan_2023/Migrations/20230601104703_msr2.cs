using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_Doan_2023.Migrations
{
    public partial class msr2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Menu_Menu_MenuId",
                table: "User_Menu");

            migrationBuilder.DropIndex(
                name: "IX_User_Menu_MenuId",
                table: "User_Menu");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_User_Menu_MenuId",
                table: "User_Menu",
                column: "MenuId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Menu_Menu_MenuId",
                table: "User_Menu",
                column: "MenuId",
                principalTable: "Menu",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
