using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseAccessor.Migrations
{
    public partial class UpdateConstraint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_ShopProducts_Quantity",
                schema: "dbo",
                table: "ShopProducts");

            migrationBuilder.AddCheckConstraint(
                name: "CK_ShopProducts_Quantity",
                schema: "dbo",
                table: "ShopProducts",
                sql: "[Quantity] >= 0");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_ShopProducts_Quantity",
                schema: "dbo",
                table: "ShopProducts");

            migrationBuilder.AddCheckConstraint(
                name: "CK_ShopProducts_Quantity",
                schema: "dbo",
                table: "ShopProducts",
                sql: "[Quantity] >= 1");
        }
    }
}
