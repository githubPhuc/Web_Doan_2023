using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_Doan_2023.Migrations
{
    public partial class msr111 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Address",
                table: "EditaccountModel",
                newName: "images");

            migrationBuilder.AddColumn<int>(
                name: "City",
                table: "EditaccountModel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "District",
                table: "EditaccountModel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Fullname",
                table: "EditaccountModel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsLocked",
                table: "EditaccountModel",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddress",
                table: "EditaccountModel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Wards",
                table: "EditaccountModel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "idDepartment",
                table: "EditaccountModel",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "EditaccountModel");

            migrationBuilder.DropColumn(
                name: "District",
                table: "EditaccountModel");

            migrationBuilder.DropColumn(
                name: "Fullname",
                table: "EditaccountModel");

            migrationBuilder.DropColumn(
                name: "IsLocked",
                table: "EditaccountModel");

            migrationBuilder.DropColumn(
                name: "ShippingAddress",
                table: "EditaccountModel");

            migrationBuilder.DropColumn(
                name: "Wards",
                table: "EditaccountModel");

            migrationBuilder.DropColumn(
                name: "idDepartment",
                table: "EditaccountModel");

            migrationBuilder.RenameColumn(
                name: "images",
                table: "EditaccountModel",
                newName: "Address");
        }
    }
}
