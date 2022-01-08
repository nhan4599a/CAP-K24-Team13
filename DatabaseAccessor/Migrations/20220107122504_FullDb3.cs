using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseAccessor.Migrations
{
    public partial class FullDb3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductComments_ProductComments_ParentId",
                table: "ProductComments");

            migrationBuilder.DropIndex(
                name: "IX_ProductComments_ParentId",
                table: "ProductComments");

            migrationBuilder.DropIndex(
                name: "IX_ProductComments_ReferenceId",
                table: "ProductComments");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "ProductComments",
                newName: "ProductCommentId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "ProductCommentId",
                table: "ProductComments",
                newName: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductComments_ParentId",
                table: "ProductComments",
                column: "ParentId",
                unique: true,
                filter: "[ParentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ProductComments_ReferenceId",
                table: "ProductComments",
                column: "ReferenceId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductComments_ProductComments_ParentId",
                table: "ProductComments",
                column: "ParentId",
                principalTable: "ProductComments",
                principalColumn: "Id");
        }
    }
}
