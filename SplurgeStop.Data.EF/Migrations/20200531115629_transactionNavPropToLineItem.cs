using Microsoft.EntityFrameworkCore.Migrations;

namespace SplurgeStop.Data.EF.Migrations
{
    public partial class transactionNavPropToLineItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LineItem_Purchases_PurchaseTransactionId",
                table: "LineItem");

            migrationBuilder.AddForeignKey(
                name: "FK_LineItem_Purchases_PurchaseTransactionId",
                table: "LineItem",
                column: "PurchaseTransactionId",
                principalTable: "Purchases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LineItem_Purchases_PurchaseTransactionId",
                table: "LineItem");

            migrationBuilder.AddForeignKey(
                name: "FK_LineItem_Purchases_PurchaseTransactionId",
                table: "LineItem",
                column: "PurchaseTransactionId",
                principalTable: "Purchases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
