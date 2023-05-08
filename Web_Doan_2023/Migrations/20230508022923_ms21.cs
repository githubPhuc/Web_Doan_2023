using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_Doan_2023.Migrations
{
    public partial class ms21 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartDetailProduct");

            migrationBuilder.RenameColumn(
                name: "idDetop",
                table: "productDepot",
                newName: "idDepot");

            migrationBuilder.RenameColumn(
                name: "userCreate",
                table: "CartProduct",
                newName: "ProductName");

            migrationBuilder.RenameColumn(
                name: "StatusMessage",
                table: "CartProduct",
                newName: "userID");

            migrationBuilder.AddColumn<int>(
                name: "QuantityProduct",
                table: "productDepot",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CartProductId",
                table: "Product",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "CartProduct",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "CartProduct",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "saleID",
                table: "CartProduct",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "salePrice",
                table: "CartProduct",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_CartProductId",
                table: "Product",
                column: "CartProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_CartProduct_CartProductId",
                table: "Product",
                column: "CartProductId",
                principalTable: "CartProduct",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_CartProduct_CartProductId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_CartProductId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "QuantityProduct",
                table: "productDepot");

            migrationBuilder.DropColumn(
                name: "CartProductId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "CartProduct");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "CartProduct");

            migrationBuilder.DropColumn(
                name: "saleID",
                table: "CartProduct");

            migrationBuilder.DropColumn(
                name: "salePrice",
                table: "CartProduct");

            migrationBuilder.RenameColumn(
                name: "idDepot",
                table: "productDepot",
                newName: "idDetop");

            migrationBuilder.RenameColumn(
                name: "userID",
                table: "CartProduct",
                newName: "StatusMessage");

            migrationBuilder.RenameColumn(
                name: "ProductName",
                table: "CartProduct",
                newName: "userCreate");

            migrationBuilder.CreateTable(
                name: "CartDetailProduct",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCartProduct = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartDetailProduct", x => x.Id);
                });
        }
    }
}
