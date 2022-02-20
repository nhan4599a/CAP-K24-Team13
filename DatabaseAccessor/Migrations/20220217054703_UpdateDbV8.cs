using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseAccessor.Migrations
{
    public partial class UpdateDbV8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Created",
                table: "Invoices",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "IX_Invoices_UserId_Created",
                table: "Invoices",
                newName: "IX_Invoices_UserId_CreatedAt");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Invoices",
                newName: "Created");

            migrationBuilder.RenameIndex(
                name: "IX_Invoices_UserId_CreatedAt",
                table: "Invoices",
                newName: "IX_Invoices_UserId_Created");
        }
    }
}
