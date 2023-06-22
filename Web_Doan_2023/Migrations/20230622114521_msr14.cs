using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_Doan_2023.Migrations
{
    public partial class msr14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "nameCategory",
                table: "Product");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "nameCategory",
                table: "Product",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
