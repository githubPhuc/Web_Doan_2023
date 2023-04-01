using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_Doan_2023.Migrations
{
    public partial class ms7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdUser",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "idDepartment",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Depot",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    codeDepot = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    nameDepot = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    storekeepers = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Depot", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User_Page",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PageId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdUsercreate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdUserupdate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    User_PageId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Page", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Page_User_Page_User_PageId",
                        column: x => x.User_PageId,
                        principalTable: "User_Page",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_Page_User_PageId",
                table: "User_Page",
                column: "User_PageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Depot");

            migrationBuilder.DropTable(
                name: "User_Page");

            migrationBuilder.DropColumn(
                name: "idDepartment",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "IdUser",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
