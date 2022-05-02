using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseAccessor.Migrations
{
    public partial class UpdatePaymentMethod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "dbo",
                table: "ShopProducts",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "getdate() + '7:0:0'");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "dbo",
                table: "Reports",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "getdate() + '7:0:0'");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "dbo",
                table: "ProductComments",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "getdate() + '7:0:0'");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ChangedDate",
                schema: "dbo",
                table: "InvoiceStatusChangedHistories",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "getdate() + '7:0:0'");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "dbo",
                table: "Invoices",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "getdate() + '7:0:0'");

            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                schema: "dbo",
                table: "Invoices",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PaymentMethod",
                schema: "dbo",
                table: "Invoices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RefId",
                schema: "dbo",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPaid",
                schema: "dbo",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                schema: "dbo",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "RefId",
                schema: "dbo",
                table: "Invoices");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "dbo",
                table: "ShopProducts",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getdate() + '7:0:0'",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "getdate()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "dbo",
                table: "Reports",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getdate() + '7:0:0'",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "getdate()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "dbo",
                table: "ProductComments",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getdate() + '7:0:0'",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "getdate()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ChangedDate",
                schema: "dbo",
                table: "InvoiceStatusChangedHistories",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getdate() + '7:0:0'",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "getdate()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "dbo",
                table: "Invoices",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getdate() + '7:0:0'",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "getdate()");
        }
    }
}
