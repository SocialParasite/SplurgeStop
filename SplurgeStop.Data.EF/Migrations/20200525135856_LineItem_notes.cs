using Microsoft.EntityFrameworkCore.Migrations;

namespace SplurgeStop.Data.EF.Migrations
{
    public partial class LineItem_notes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "LineItem",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notes",
                table: "LineItem");
        }
    }
}
