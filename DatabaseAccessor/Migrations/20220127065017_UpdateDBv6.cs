using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseAccessor.Migrations
{
    public partial class UpdateDBv6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ShippingAddress",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Invoices");

            migrationBuilder.AlterColumn<string>(
                name: "ShippingAddress",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
