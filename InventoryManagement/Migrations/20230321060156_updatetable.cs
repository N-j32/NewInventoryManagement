using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagement.Migrations
{
    public partial class updatetable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Supplier_SupplierId",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_SupplierId",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "OrderDetails");

            migrationBuilder.AddColumn<int>(
                name: "SupplierId",
                table: "Order",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Order_SupplierId",
                table: "Order",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Supplier_SupplierId",
                table: "Order",
                column: "SupplierId",
                principalTable: "Supplier",
                principalColumn: "SupplierId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Supplier_SupplierId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_SupplierId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "Order");

            migrationBuilder.AddColumn<int>(
                name: "SupplierId",
                table: "OrderDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_SupplierId",
                table: "OrderDetails",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Supplier_SupplierId",
                table: "OrderDetails",
                column: "SupplierId",
                principalTable: "Supplier",
                principalColumn: "SupplierId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
