using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseAccessor.Migrations
{
    public partial class RedesignInvoice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsCommented",
                schema: "dbo",
                table: "InvoiceDetails",
                newName: "IsRated");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsRated",
                schema: "dbo",
                table: "InvoiceDetails",
                newName: "IsCommented");
        }
    }
}
