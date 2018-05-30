using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Kastra.DAL.EntityFramework.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Kastra_Module_Definitions",
                columns: table => new
                {
                    ModuleDefID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    KeyName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Namespace = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Path = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kastra_Module_Definitions", x => x.ModuleDefID);
                });

            migrationBuilder.CreateTable(
                name: "Kastra_Page_Templates",
                columns: table => new
                {
                    PageTemplateID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    KeyName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Path = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kastra_Page_Templates", x => x.PageTemplateID);
                });

            migrationBuilder.CreateTable(
                name: "Kastra_Parameters",
                columns: table => new
                {
                    ParameterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Key = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Value = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kastra_Parameters", x => x.ParameterId);
                });

            migrationBuilder.CreateTable(
                name: "Kastra_Permissions",
                columns: table => new
                {
                    PermissionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kastra_Permissions", x => x.PermissionID);
                });

            migrationBuilder.CreateTable(
                name: "Kastra_Module_Controls",
                columns: table => new
                {
                    ModuleControlID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    KeyName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    ModuleDefID = table.Column<int>(type: "int", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kastra_Module_Controls", x => x.ModuleControlID);
                    table.ForeignKey(
                        name: "FK_Kastra_Module_Controls_Kastra_Modules",
                        column: x => x.ModuleDefID,
                        principalTable: "Kastra_Module_Definitions",
                        principalColumn: "ModuleDefID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Kastra_Pages",
                columns: table => new
                {
                    PageID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    KeyName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MetaDescription = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    MetaKeywords = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    MetaRobot = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PageTemplateID = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kastra_Pages", x => x.PageID);
                    table.ForeignKey(
                        name: "FK_Kastra_Pages_Kastra_Page_Templates",
                        column: x => x.PageTemplateID,
                        principalTable: "Kastra_Page_Templates",
                        principalColumn: "PageTemplateID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Kastra_Places",
                columns: table => new
                {
                    PlaceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    KeyName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PageTemplateID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kastra_Places", x => x.PlaceID);
                    table.ForeignKey(
                        name: "FK_Kastra_Places_Kastra_Page_Templates",
                        column: x => x.PageTemplateID,
                        principalTable: "Kastra_Page_Templates",
                        principalColumn: "PageTemplateID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Kastra_Modules",
                columns: table => new
                {
                    ModuleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ModuleDefID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PageID = table.Column<int>(type: "int", nullable: false),
                    PlaceID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kastra_Modules", x => x.ModuleID);
                    table.ForeignKey(
                        name: "FK_Kastra_Modules_Kastra_Module_Definitions",
                        column: x => x.ModuleDefID,
                        principalTable: "Kastra_Module_Definitions",
                        principalColumn: "ModuleDefID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Kastra_Modules_Kastra_Places",
                        column: x => x.PlaceID,
                        principalTable: "Kastra_Places",
                        principalColumn: "PlaceID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Kastra_Module_Permissions",
                columns: table => new
                {
                    ModulePermissionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ModuleID = table.Column<int>(type: "int", nullable: false),
                    PermissionID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kastra_Module_Permissions", x => x.ModulePermissionID);
                    table.ForeignKey(
                        name: "FK_Kastra_Module_Permissions_Kastra_Modules",
                        column: x => x.ModuleID,
                        principalTable: "Kastra_Modules",
                        principalColumn: "ModuleID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Kastra_Module_Permissions_Kastra_Module_Permissions",
                        column: x => x.PermissionID,
                        principalTable: "Kastra_Permissions",
                        principalColumn: "PermissionID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Kastra_Module_Controls_ModuleDefID",
                table: "Kastra_Module_Controls",
                column: "ModuleDefID");

            migrationBuilder.CreateIndex(
                name: "IX_Kastra_Module_Permissions_ModuleID",
                table: "Kastra_Module_Permissions",
                column: "ModuleID");

            migrationBuilder.CreateIndex(
                name: "IX_Kastra_Module_Permissions_PermissionID",
                table: "Kastra_Module_Permissions",
                column: "PermissionID");

            migrationBuilder.CreateIndex(
                name: "IX_Kastra_Modules_ModuleDefID",
                table: "Kastra_Modules",
                column: "ModuleDefID");

            migrationBuilder.CreateIndex(
                name: "IX_Kastra_Modules_PlaceID",
                table: "Kastra_Modules",
                column: "PlaceID");

            migrationBuilder.CreateIndex(
                name: "IX_Kastra_Pages_PageTemplateID",
                table: "Kastra_Pages",
                column: "PageTemplateID");

            migrationBuilder.CreateIndex(
                name: "IX_Kastra_Places_PageTemplateID",
                table: "Kastra_Places",
                column: "PageTemplateID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Kastra_Module_Controls");

            migrationBuilder.DropTable(
                name: "Kastra_Module_Permissions");

            migrationBuilder.DropTable(
                name: "Kastra_Pages");

            migrationBuilder.DropTable(
                name: "Kastra_Parameters");

            migrationBuilder.DropTable(
                name: "Kastra_Modules");

            migrationBuilder.DropTable(
                name: "Kastra_Permissions");

            migrationBuilder.DropTable(
                name: "Kastra_Module_Definitions");

            migrationBuilder.DropTable(
                name: "Kastra_Places");

            migrationBuilder.DropTable(
                name: "Kastra_Page_Templates");
        }
    }
}
