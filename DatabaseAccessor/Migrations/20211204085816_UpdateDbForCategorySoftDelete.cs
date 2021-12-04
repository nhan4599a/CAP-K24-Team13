using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseAccessor.Migrations
{
    public partial class UpdateDbForCategorySoftDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDisabled",
                table: "ShopCategories",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDisabled",
                table: "ShopCategories");
        }
    }
}
