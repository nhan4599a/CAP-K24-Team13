using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseAccessor.Migrations
{
    public partial class RemoveRedundantField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Special",
                schema: "dbo",
                table: "ShopCategories");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Special",
                schema: "dbo",
                table: "ShopCategories",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
