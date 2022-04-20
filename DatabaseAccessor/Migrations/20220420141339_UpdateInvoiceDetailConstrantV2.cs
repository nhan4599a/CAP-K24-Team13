﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseAccessor.Migrations
{
    public partial class UpdateInvoiceDetailConstrantV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_InvoiceDetail_Quantity",
                schema: "dbo",
                table: "InvoiceDetails");

            migrationBuilder.AddCheckConstraint(
                name: "CK_InvoiceDetail_Quantity",
                schema: "dbo",
                table: "InvoiceDetails",
                sql: "[Quantity] >= 1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_InvoiceDetail_Quantity",
                schema: "dbo",
                table: "InvoiceDetails");

            migrationBuilder.AddCheckConstraint(
                name: "CK_InvoiceDetail_Quantity",
                schema: "dbo",
                table: "InvoiceDetails",
                sql: "[Quantity] > 1");
        }
    }
}
