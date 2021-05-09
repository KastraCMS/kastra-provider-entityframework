using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Kastra.DAL.EntityFramework.Migrations
{
    public partial class Initial_Kastra_200 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    DisplayedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Kastra_Files",
                columns: table => new
                {
                    FileID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Path = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kastra_Files", x => x.FileID);
                });

            migrationBuilder.CreateTable(
                name: "Kastra_Mail_Templates",
                columns: table => new
                {
                    MailTemplateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Keyname = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kastra_Mail_Templates", x => x.MailTemplateId);
                });

            migrationBuilder.CreateTable(
                name: "Kastra_Module_Definitions",
                columns: table => new
                {
                    ModuleDefID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Path = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    KeyName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Namespace = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Version = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
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
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KeyName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ViewPath = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ModelClass = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
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
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Key = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Value = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
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
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kastra_Permissions", x => x.PermissionID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Kastra_Visitors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastVisitAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kastra_Visitors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Kastra_Visitors_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Kastra_Module_Controls",
                columns: table => new
                {
                    ModuleControlID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModuleDefID = table.Column<int>(type: "int", nullable: false),
                    KeyName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
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
                name: "Kastra_Module_Navigations",
                columns: table => new
                {
                    ModuleNavigationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ModuleDefinitionID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kastra_Module_Navigations", x => x.ModuleNavigationID);
                    table.ForeignKey(
                        name: "FK_Kastra_Module_Navigations_Kastra_Module!Definitions",
                        column: x => x.ModuleDefinitionID,
                        principalTable: "Kastra_Module_Definitions",
                        principalColumn: "ModuleDefID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Kastra_Pages",
                columns: table => new
                {
                    PageID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PageTemplateID = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    KeyName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MetaDescription = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: true),
                    MetaKeywords = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    MetaRobot = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
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
                name: "Kastra_Modules",
                columns: table => new
                {
                    ModuleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModuleDefID = table.Column<int>(type: "int", nullable: false),
                    PlaceID = table.Column<int>(type: "int", nullable: false),
                    PageID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "Kastra_Module_Permissions",
                columns: table => new
                {
                    ModulePermissionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PermissionID = table.Column<int>(type: "int", nullable: false),
                    ModuleID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kastra_Module_Permissions", x => x.ModulePermissionID);
                    table.ForeignKey(
                        name: "FK_Kastra_Module_Permissions_Kastra_Module_Permissions",
                        column: x => x.PermissionID,
                        principalTable: "Kastra_Permissions",
                        principalColumn: "PermissionID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Kastra_Module_Permissions_Kastra_Modules",
                        column: x => x.ModuleID,
                        principalTable: "Kastra_Modules",
                        principalColumn: "ModuleID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Kastra_Places",
                columns: table => new
                {
                    PlaceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PageTemplateID = table.Column<int>(type: "int", nullable: false),
                    KeyName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ModuleId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kastra_Places", x => x.PlaceID);
                    table.ForeignKey(
                        name: "FK_Kastra_Places_Kastra_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Kastra_Modules",
                        principalColumn: "ModuleID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Kastra_Places_Kastra_Page_Templates",
                        column: x => x.PageTemplateID,
                        principalTable: "Kastra_Page_Templates",
                        principalColumn: "PageTemplateID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Kastra_Module_Controls_ModuleDefID",
                table: "Kastra_Module_Controls",
                column: "ModuleDefID");

            migrationBuilder.CreateIndex(
                name: "IX_Kastra_Module_Navigations_ModuleDefinitionID",
                table: "Kastra_Module_Navigations",
                column: "ModuleDefinitionID");

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
                name: "IX_Kastra_Places_ModuleId",
                table: "Kastra_Places",
                column: "ModuleId",
                unique: true,
                filter: "[ModuleId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Kastra_Places_PageTemplateID",
                table: "Kastra_Places",
                column: "PageTemplateID");

            migrationBuilder.CreateIndex(
                name: "IX_Kastra_Visitors_UserId",
                table: "Kastra_Visitors",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Kastra_Modules_Kastra_Places",
                table: "Kastra_Modules",
                column: "PlaceID",
                principalTable: "Kastra_Places",
                principalColumn: "PlaceID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kastra_Modules_Kastra_Module_Definitions",
                table: "Kastra_Modules");

            migrationBuilder.DropForeignKey(
                name: "FK_Kastra_Places_Kastra_Modules_ModuleId",
                table: "Kastra_Places");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Kastra_Files");

            migrationBuilder.DropTable(
                name: "Kastra_Mail_Templates");

            migrationBuilder.DropTable(
                name: "Kastra_Module_Controls");

            migrationBuilder.DropTable(
                name: "Kastra_Module_Navigations");

            migrationBuilder.DropTable(
                name: "Kastra_Module_Permissions");

            migrationBuilder.DropTable(
                name: "Kastra_Pages");

            migrationBuilder.DropTable(
                name: "Kastra_Parameters");

            migrationBuilder.DropTable(
                name: "Kastra_Visitors");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Kastra_Permissions");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Kastra_Module_Definitions");

            migrationBuilder.DropTable(
                name: "Kastra_Modules");

            migrationBuilder.DropTable(
                name: "Kastra_Places");

            migrationBuilder.DropTable(
                name: "Kastra_Page_Templates");
        }
    }
}
