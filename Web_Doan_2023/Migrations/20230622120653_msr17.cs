using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_Doan_2023.Migrations
{
    public partial class msr17 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PathImage",
                table: "Images",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PathImage",
                table: "Images");
        }
    }
}
