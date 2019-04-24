using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Kastra.DAL.EntityFramework.Migrations
{
    public partial class Kastra_0_09_14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Path",
                table: "Kastra_Page_Templates",
                newName: "ViewPath");

            migrationBuilder.AddColumn<string>(
                name: "ModelClass",
                table: "Kastra_Page_Templates",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Kastra_Files",
                columns: table => new
                {
                    FileID = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 150, nullable: true),
                    Path = table.Column<string>(maxLength: 500, nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2019, 4, 20, 17, 7, 21, 338, DateTimeKind.Local))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kastra_Files", x => x.FileID);
                });

            migrationBuilder.Sql("UPDATE Kastra_Page_Templates SET ModelClass = CONCAT(ViewPath, '.', Keyname, 'ViewModel')");
            migrationBuilder.Sql("UPDATE Kastra_Page_Templates SET ViewPath = 'Page'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Kastra_Files");

            migrationBuilder.RenameColumn(
                name: "ViewPath",
                table: "Kastra_Page_Templates",
                newName: "Path");

            migrationBuilder.Sql("UPDATE Kastra_Page_Templates SET Path = REPLACE(ModelClass, CONCAT('.', Keyname, 'ViewModel'), '') WHERE KeyName IN ('DefaultTemplate', 'HomeTemplate')");

            migrationBuilder.DropColumn(
                name: "ModelClass",
                table: "Kastra_Page_Templates");
        }
    }
}
