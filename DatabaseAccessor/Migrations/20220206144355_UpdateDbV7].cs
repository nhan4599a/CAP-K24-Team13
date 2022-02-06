using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseAccessor.Migrations
{
    public partial class UpdateDbV7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductComments_ProductComments_ReferenceId",
                table: "ProductComments");

            migrationBuilder.DropIndex(
                name: "IX_ProductComments_ReferenceId",
                table: "ProductComments");

            migrationBuilder.DropColumn(
                name: "ReferenceId",
                table: "ProductComments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReferenceId",
                table: "ProductComments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductComments_ReferenceId",
                table: "ProductComments",
                column: "ReferenceId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductComments_ProductComments_ReferenceId",
                table: "ProductComments",
                column: "ReferenceId",
                principalTable: "ProductComments",
                principalColumn: "Id");
        }
    }
}
