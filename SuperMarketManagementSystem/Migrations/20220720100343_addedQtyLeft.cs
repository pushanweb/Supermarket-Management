using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperMarketManagementSystem.Migrations
{
    public partial class addedQtyLeft : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QtyLeft",
                table: "Invoice",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QtyLeft",
                table: "Invoice");
        }
    }
}
