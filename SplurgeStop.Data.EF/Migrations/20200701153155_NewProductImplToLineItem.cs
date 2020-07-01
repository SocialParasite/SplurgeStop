using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SplurgeStop.Data.EF.Migrations
{
    public partial class NewProductImplToLineItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "LineItem",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LineItem_ProductId",
                table: "LineItem",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_LineItem_Products_ProductId",
                table: "LineItem",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LineItem_Products_ProductId",
                table: "LineItem");

            migrationBuilder.DropIndex(
                name: "IX_LineItem_ProductId",
                table: "LineItem");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "LineItem");
        }
    }
}
