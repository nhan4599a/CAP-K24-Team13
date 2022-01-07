using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseAccessor.Migrations
{
    public partial class FullDbV3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductComments_ProductComments_ProductCommentId",
                table: "ProductComments");

            migrationBuilder.DropIndex(
                name: "IX_ProductComments_ProductCommentId",
                table: "ProductComments");

            migrationBuilder.DropIndex(
                name: "IX_ProductComments_ReferenceId",
                table: "ProductComments");

            migrationBuilder.DropColumn(
                name: "ProductCommentId",
                table: "ProductComments");

            migrationBuilder.CreateIndex(
                name: "IX_ProductComments_ReferenceId",
                table: "ProductComments",
                column: "ReferenceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductComments_ReferenceId",
                table: "ProductComments");

            migrationBuilder.AddColumn<int>(
                name: "ProductCommentId",
                table: "ProductComments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductComments_ProductCommentId",
                table: "ProductComments",
                column: "ProductCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductComments_ReferenceId",
                table: "ProductComments",
                column: "ReferenceId",
                unique: true,
                filter: "[ReferenceId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductComments_ProductComments_ProductCommentId",
                table: "ProductComments",
                column: "ProductCommentId",
                principalTable: "ProductComments",
                principalColumn: "Id");
        }
    }
}
