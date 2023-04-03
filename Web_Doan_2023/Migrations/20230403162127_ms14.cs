using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_Doan_2023.Migrations
{
    public partial class ms14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Page_User_Page_User_PageId",
                table: "User_Page");

            migrationBuilder.DropIndex(
                name: "IX_User_Page_User_PageId",
                table: "User_Page");

            migrationBuilder.DropColumn(
                name: "User_PageId",
                table: "User_Page");

            migrationBuilder.AlterColumn<string>(
                name: "IdUserupdate",
                table: "Page",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "IdUsercreate",
                table: "Page",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "User_PageId",
                table: "User_Page",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IdUserupdate",
                table: "Page",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IdUsercreate",
                table: "Page",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Page_User_PageId",
                table: "User_Page",
                column: "User_PageId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Page_User_Page_User_PageId",
                table: "User_Page",
                column: "User_PageId",
                principalTable: "User_Page",
                principalColumn: "Id");
        }
    }
}
