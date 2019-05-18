using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Kastra.DAL.EntityFramework.Migrations
{
    public partial class Kastra_0_09_15 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "Kastra_Parameters",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "Kastra_Files",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 4, 20, 17, 7, 21, 338, DateTimeKind.Local));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "Kastra_Parameters",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "Kastra_Files",
                nullable: false,
                defaultValue: new DateTime(2019, 4, 20, 17, 7, 21, 338, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "getdate()");
        }
    }
}
