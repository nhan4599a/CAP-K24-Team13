using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseAccessor.Migrations
{
    public partial class UpdateDbV9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "CartDetail");

            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.RenameTable(
                name: "ShopProducts",
                newName: "ShopProducts",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "ShopInterfaces",
                newName: "ShopInterfaces",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "ShopCategories",
                newName: "ShopCategories",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "ProductComments",
                newName: "ProductComments",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "InvoiceStatusChangedHistories",
                newName: "InvoiceStatusChangedHistories",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Invoices",
                newName: "Invoices",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "InvoiceDetails",
                newName: "InvoiceDetails",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Carts",
                newName: "Carts",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "CartDetails",
                newName: "CartDetails",
                newSchema: "CartDetail");

            migrationBuilder.AddColumn<int>(
                name: "ShopId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShopId",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "ShopProducts",
                schema: "dbo",
                newName: "ShopProducts");

            migrationBuilder.RenameTable(
                name: "ShopInterfaces",
                schema: "dbo",
                newName: "ShopInterfaces");

            migrationBuilder.RenameTable(
                name: "ShopCategories",
                schema: "dbo",
                newName: "ShopCategories");

            migrationBuilder.RenameTable(
                name: "ProductComments",
                schema: "dbo",
                newName: "ProductComments");

            migrationBuilder.RenameTable(
                name: "InvoiceStatusChangedHistories",
                schema: "dbo",
                newName: "InvoiceStatusChangedHistories");

            migrationBuilder.RenameTable(
                name: "Invoices",
                schema: "dbo",
                newName: "Invoices");

            migrationBuilder.RenameTable(
                name: "InvoiceDetails",
                schema: "dbo",
                newName: "InvoiceDetails");

            migrationBuilder.RenameTable(
                name: "Carts",
                schema: "dbo",
                newName: "Carts");

            migrationBuilder.RenameTable(
                name: "CartDetails",
                schema: "CartDetail",
                newName: "CartDetails");
        }
    }
}
