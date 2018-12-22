using Microsoft.EntityFrameworkCore.Migrations;

namespace DrankReusapi.Migrations
{
    public partial class AddUserRemoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Removed",
                table: "Users",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "Removed",
                table: "Product",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Removed",
                table: "Users");

            migrationBuilder.AlterColumn<bool>(
                name: "Removed",
                table: "Product",
                nullable: false,
                oldClrType: typeof(bool),
                oldDefaultValue: false);
        }
    }
}
