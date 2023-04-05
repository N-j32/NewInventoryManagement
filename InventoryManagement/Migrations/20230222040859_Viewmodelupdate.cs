using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagement.Migrations
{
    public partial class Viewmodelupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gender",
                table: "EmployeeRegisters");

            migrationBuilder.AlterColumn<long>(
                name: "Number",
                table: "EmployeeRegisters",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "EmployeeRegisters",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRemember",
                table: "EmployeeRegisters",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "EmployeeRegisters");

            migrationBuilder.DropColumn(
                name: "IsRemember",
                table: "EmployeeRegisters");

            migrationBuilder.AlterColumn<string>(
                name: "Number",
                table: "EmployeeRegisters",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "EmployeeRegisters",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
