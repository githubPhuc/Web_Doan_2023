using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_Doan_2023.Migrations
{
    public partial class msr1234 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "CartProduct");

            migrationBuilder.DropColumn(
                name: "idProductDeport",
                table: "CartProduct");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreate",
                table: "productDepot",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ShipmentCode",
                table: "CartProduct",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreate",
                table: "productDepot");

            migrationBuilder.DropColumn(
                name: "ShipmentCode",
                table: "CartProduct");

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "CartProduct",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "idProductDeport",
                table: "CartProduct",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
