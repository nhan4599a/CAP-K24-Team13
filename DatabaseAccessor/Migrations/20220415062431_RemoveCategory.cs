using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseAccessor.Migrations
{
    public partial class RemoveCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShopProducts_ShopCategories_CategoryId",
                schema: "dbo",
                table: "ShopProducts");

            migrationBuilder.DropTable(
                name: "ShopCategories",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_ShopProducts_CategoryId",
                schema: "dbo",
                table: "ShopProducts");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                schema: "dbo",
                table: "ShopProducts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsVisible",
                schema: "dbo",
                table: "ShopProducts",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                schema: "dbo",
                table: "ShopProducts");

            migrationBuilder.DropColumn(
                name: "IsVisible",
                schema: "dbo",
                table: "ShopProducts");

            migrationBuilder.CreateTable(
                name: "ShopCategories",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ShopId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopCategories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShopProducts_CategoryId",
                schema: "dbo",
                table: "ShopProducts",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopCategories_CategoryName",
                schema: "dbo",
                table: "ShopCategories",
                column: "CategoryName");

            migrationBuilder.AddForeignKey(
                name: "FK_ShopProducts_ShopCategories_CategoryId",
                schema: "dbo",
                table: "ShopProducts",
                column: "CategoryId",
                principalSchema: "dbo",
                principalTable: "ShopCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
