using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseAccessor.Migrations
{
    public partial class FixDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "ShopProducts",
                type: "int",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ProductName",
                table: "ShopProducts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Discount",
                table: "ShopProducts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ShopProducts",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2021, 11, 29, 12, 9, 3, 888, DateTimeKind.Local).AddTicks(7325));

            migrationBuilder.CreateIndex(
                name: "IX_ShopProducts_ProductName",
                table: "ShopProducts",
                column: "ProductName");

            migrationBuilder.AddCheckConstraint(
                name: "CK_ShopProducts_Price",
                table: "ShopProducts",
                sql: "[Price] >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_ShopProducts_Quantity",
                table: "ShopProducts",
                sql: "[Quantity] >= 1");

            migrationBuilder.AddCheckConstraint(
                name: "CK_ShopProducts_Discount",
                table: "ShopProducts",
                sql: "[Discount] between 0 and 100");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ShopProducts_ProductName",
                table: "ShopProducts");

            migrationBuilder.DropCheckConstraint(
                name: "CK_ShopProducts_Price",
                table: "ShopProducts");

            migrationBuilder.DropCheckConstraint(
                name: "CK_ShopProducts_Quantity",
                table: "ShopProducts");

            migrationBuilder.DropCheckConstraint(
                name: "CK_ShopProducts_Discount",
                table: "ShopProducts");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "ShopProducts",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 1);

            migrationBuilder.AlterColumn<string>(
                name: "ProductName",
                table: "ShopProducts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "Discount",
                table: "ShopProducts",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ShopProducts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 11, 29, 12, 9, 3, 888, DateTimeKind.Local).AddTicks(7325),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "getdate()");
        }
    }
}
