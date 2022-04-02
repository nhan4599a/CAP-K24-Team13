using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseAccessor.Migrations
{
    public partial class ChangeSchemeName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "CartDetails",
                schema: "CartDetail",
                newName: "CartDetails",
                newSchema: "dbo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "CartDetail");

            migrationBuilder.RenameTable(
                name: "CartDetails",
                schema: "dbo",
                newName: "CartDetails",
                newSchema: "CartDetail");
        }
    }
}
