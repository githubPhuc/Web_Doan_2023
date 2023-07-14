using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_Doan_2023.Migrations
{
    public partial class msrt123 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Idproduct",
                table: "BillOfSaleDetail",
                newName: "IdProduct");

            migrationBuilder.AddColumn<string>(
                name: "ShipmentCode",
                table: "BillOfSaleDetail",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShipmentCode",
                table: "BillOfSaleDetail");

            migrationBuilder.RenameColumn(
                name: "IdProduct",
                table: "BillOfSaleDetail",
                newName: "Idproduct");
        }
    }
}
