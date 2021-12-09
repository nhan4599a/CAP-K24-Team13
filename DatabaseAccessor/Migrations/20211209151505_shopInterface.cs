using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseAccessor.Migrations
{
    public partial class shopInterface : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Option",
                table: "ShopInterfaces");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "ShopInterfaces",
                newName: "ShopPhoneNumber");

            migrationBuilder.AddColumn<string>(
                name: "ShopAddress",
                table: "ShopInterfaces",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShopDescription",
                table: "ShopInterfaces",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShopName",
                table: "ShopInterfaces",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CategoryName",
                table: "ShopCategories",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShopInterfaces_ShopId",
                table: "ShopInterfaces",
                column: "ShopId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShopCategories_CategoryName",
                table: "ShopCategories",
                column: "CategoryName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ShopInterfaces_ShopId",
                table: "ShopInterfaces");

            migrationBuilder.DropIndex(
                name: "IX_ShopCategories_CategoryName",
                table: "ShopCategories");

            migrationBuilder.DropColumn(
                name: "ShopAddress",
                table: "ShopInterfaces");

            migrationBuilder.DropColumn(
                name: "ShopDescription",
                table: "ShopInterfaces");

            migrationBuilder.DropColumn(
                name: "ShopName",
                table: "ShopInterfaces");

            migrationBuilder.RenameColumn(
                name: "ShopPhoneNumber",
                table: "ShopInterfaces",
                newName: "Description");

            migrationBuilder.AddColumn<int>(
                name: "Option",
                table: "ShopInterfaces",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "CategoryName",
                table: "ShopCategories",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
