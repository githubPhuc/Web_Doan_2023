using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_Doan_2023.Migrations
{
    public partial class msjj1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "enterWarehouseVouchers");

            migrationBuilder.DropTable(
                name: "exportWarehouseVouchers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "enterWarehouseVouchers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(type: "int", nullable: true),
                    WarehouseId = table.Column<int>(type: "int", nullable: true),
                    agree = table.Column<bool>(type: "bit", nullable: false),
                    codeEnterWarehouseVouchers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    dateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    dateDelete = table.Column<DateTime>(type: "datetime2", nullable: true),
                    dateUpdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    priceEnterWarehouseVouchers = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    userCreate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    userDelete = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    userUpdate = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_enterWarehouseVouchers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "exportWarehouseVouchers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(type: "int", nullable: true),
                    WarehouseId = table.Column<int>(type: "int", nullable: true),
                    agree = table.Column<bool>(type: "bit", nullable: false),
                    codeExportWarehouseVouchers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    dateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    dateDelete = table.Column<DateTime>(type: "datetime2", nullable: true),
                    dateUpdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    priceExportWarehouseVouchers = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    userCreate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    userDelete = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    userUpdate = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exportWarehouseVouchers", x => x.Id);
                });
        }
    }
}
