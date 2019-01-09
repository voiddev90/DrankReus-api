using Microsoft.EntityFrameworkCore.Migrations;

namespace DrankReusapi.Migrations
{
    public partial class AddUserDiscountPoints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DiscountPoints",
                table: "Users",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountPoints",
                table: "Users");
        }
    }
}
