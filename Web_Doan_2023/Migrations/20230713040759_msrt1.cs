using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_Doan_2023.Migrations
{
    public partial class msrt1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "numLo",
                table: "productDepot");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "CartProduct",
                newName: "idProductDeport");

            migrationBuilder.AddColumn<string>(
                name: "ShipmentCode",
                table: "productDepot",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "status",
                table: "productDepot",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShipmentCode",
                table: "productDepot");

            migrationBuilder.DropColumn(
                name: "status",
                table: "productDepot");

            migrationBuilder.RenameColumn(
                name: "idProductDeport",
                table: "CartProduct",
                newName: "ProductId");

            migrationBuilder.AddColumn<int>(
                name: "numLo",
                table: "productDepot",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
