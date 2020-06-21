using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SplurgeStop.Data.EF.Migrations
{
    public partial class StoreLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LocationId",
                table: "Stores",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stores_LocationId",
                table: "Stores",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stores_Locations_LocationId",
                table: "Stores",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stores_Locations_LocationId",
                table: "Stores");

            migrationBuilder.DropIndex(
                name: "IX_Stores_LocationId",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Stores");
        }
    }
}
