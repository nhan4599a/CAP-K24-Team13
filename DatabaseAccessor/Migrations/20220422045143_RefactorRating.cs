using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseAccessor.Migrations
{
    public partial class RefactorRating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCommented",
                schema: "dbo",
                table: "InvoiceDetails",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCommented",
                schema: "dbo",
                table: "InvoiceDetails");
        }
    }
}
