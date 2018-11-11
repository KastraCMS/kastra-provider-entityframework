using Microsoft.EntityFrameworkCore.Migrations;

namespace Kastra.DAL.EntityFramework.Migrations
{
    public partial class Kastra_0_09_08 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ModuleId",
                table: "Kastra_Places",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Kastra_Places_ModuleId",
                table: "Kastra_Places",
                column: "ModuleId",
                unique: true,
                filter: "[ModuleId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Kastra_Places_Kastra_Modules_ModuleId",
                table: "Kastra_Places",
                column: "ModuleId",
                principalTable: "Kastra_Modules",
                principalColumn: "ModuleID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kastra_Places_Kastra_Modules_ModuleId",
                table: "Kastra_Places");

            migrationBuilder.DropIndex(
                name: "IX_Kastra_Places_ModuleId",
                table: "Kastra_Places");

            migrationBuilder.DropColumn(
                name: "ModuleId",
                table: "Kastra_Places");
        }
    }
}
