using Microsoft.EntityFrameworkCore.Migrations;

namespace DrankReusapi.Migrations
{
    public partial class AddProductInventory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<int>(
                name: "Inventory",
                table: "Product",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Inventory",
                table: "Product");

            migrationBuilder.RenameColumn(
                name: "Prefix",
                table: "Users",
                newName: "prefix");
        }
    }
}
