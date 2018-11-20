using Microsoft.EntityFrameworkCore.Migrations;

namespace Kastra.DAL.EntityFramework.Migrations
{
    public partial class Kastra_0_09_09 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDisabled",
                table: "Kastra_Modules",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDisabled",
                table: "Kastra_Modules");
        }
    }
}
