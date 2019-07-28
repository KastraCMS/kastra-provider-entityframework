using Microsoft.EntityFrameworkCore.Migrations;

namespace Kastra.DAL.EntityFramework.Migrations
{
    public partial class Kastra_0_09_20 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Kastra_Mail_Templates",
                newName: "Message");

            migrationBuilder.AddColumn<string>(
                name: "Subject",
                table: "Kastra_Mail_Templates",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Subject",
                table: "Kastra_Mail_Templates");

            migrationBuilder.RenameColumn(
                name: "Message",
                table: "Kastra_Mail_Templates",
                newName: "Value");
        }
    }
}
