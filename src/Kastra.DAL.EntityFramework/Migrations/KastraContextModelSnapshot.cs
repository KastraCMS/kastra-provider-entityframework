﻿// <auto-generated />
using System;
using Kastra.DAL.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Kastra.DAL.EntityFramework.Migrations
{
    [DbContext(typeof(KastraContext))]
    partial class KastraContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Kastra.DAL.EntityFramework.Models.KastraFiles", b =>
                {
                    b.Property<Guid>("FileId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("FileID");

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("getdate()");

                    b.Property<string>("Name")
                        .HasMaxLength(150);

                    b.Property<string>("Path")
                        .HasMaxLength(500);

                    b.HasKey("FileId")
                        .HasName("PK_Kastra_Files");

                    b.ToTable("Kastra_Files");
                });

            modelBuilder.Entity("Kastra.DAL.EntityFramework.Models.KastraModuleControls", b =>
                {
                    b.Property<int>("ModuleControlId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ModuleControlID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("KeyName")
                        .HasMaxLength(150);

                    b.Property<int>("ModuleDefId")
                        .HasColumnName("ModuleDefID");

                    b.Property<string>("Path")
                        .HasMaxLength(250);

                    b.HasKey("ModuleControlId")
                        .HasName("PK_Kastra_Module_Controls");

                    b.HasIndex("ModuleDefId");

                    b.ToTable("Kastra_Module_Controls");
                });

            modelBuilder.Entity("Kastra.DAL.EntityFramework.Models.KastraModuleDefinitions", b =>
                {
                    b.Property<int>("ModuleDefId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ModuleDefID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("KeyName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<string>("Namespace")
                        .HasMaxLength(150);

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("ModuleDefId")
                        .HasName("PK_Kastra_Module_Definitions");

                    b.ToTable("Kastra_Module_Definitions");
                });

            modelBuilder.Entity("Kastra.DAL.EntityFramework.Models.KastraModulePermissions", b =>
                {
                    b.Property<int>("ModulePermissionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ModulePermissionID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ModuleId")
                        .HasColumnName("ModuleID");

                    b.Property<int>("PermissionId")
                        .HasColumnName("PermissionID");

                    b.HasKey("ModulePermissionId")
                        .HasName("PK_Kastra_Module_Permissions");

                    b.HasIndex("ModuleId");

                    b.HasIndex("PermissionId");

                    b.ToTable("Kastra_Module_Permissions");
                });

            modelBuilder.Entity("Kastra.DAL.EntityFramework.Models.KastraModules", b =>
                {
                    b.Property<int>("ModuleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ModuleID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsDisabled")
                        .HasColumnName("IsDisabled");

                    b.Property<int>("ModuleDefId")
                        .HasColumnName("ModuleDefID");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<int>("PageId")
                        .HasColumnName("PageID");

                    b.Property<int>("PlaceId")
                        .HasColumnName("PlaceID");

                    b.HasKey("ModuleId")
                        .HasName("PK_Kastra_Modules");

                    b.HasIndex("ModuleDefId");

                    b.HasIndex("PlaceId");

                    b.ToTable("Kastra_Modules");
                });

            modelBuilder.Entity("Kastra.DAL.EntityFramework.Models.KastraPages", b =>
                {
                    b.Property<int>("PageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("PageID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("KeyName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("MetaDescription")
                        .HasMaxLength(160);

                    b.Property<string>("MetaKeywords")
                        .HasMaxLength(250);

                    b.Property<string>("MetaRobot")
                        .HasMaxLength(50);

                    b.Property<int>("PageTemplateId")
                        .HasColumnName("PageTemplateID");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.HasKey("PageId")
                        .HasName("PK_Kastra_Pages");

                    b.HasIndex("PageTemplateId");

                    b.ToTable("Kastra_Pages");
                });

            modelBuilder.Entity("Kastra.DAL.EntityFramework.Models.KastraPageTemplates", b =>
                {
                    b.Property<int>("PageTemplateId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("PageTemplateID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("KeyName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("ModelClass")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<string>("ViewPath")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.HasKey("PageTemplateId")
                        .HasName("PK_Kastra_Page_Templates");

                    b.ToTable("Kastra_Page_Templates");
                });

            modelBuilder.Entity("Kastra.DAL.EntityFramework.Models.KastraParameters", b =>
                {
                    b.Property<int>("ParameterId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Key")
                        .HasMaxLength(50);

                    b.Property<string>("Name")
                        .HasMaxLength(150);

                    b.Property<string>("Value")
                        .HasMaxLength(1000);

                    b.HasKey("ParameterId")
                        .HasName("PK_Kastra_Parameters");

                    b.ToTable("Kastra_Parameters");
                });

            modelBuilder.Entity("Kastra.DAL.EntityFramework.Models.KastraPermissions", b =>
                {
                    b.Property<int>("PermissionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("PermissionID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.HasKey("PermissionId")
                        .HasName("PK_Kastra_Permissions");

                    b.ToTable("Kastra_Permissions");
                });

            modelBuilder.Entity("Kastra.DAL.EntityFramework.Models.KastraPlaces", b =>
                {
                    b.Property<int>("PlaceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("PlaceID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("KeyName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int?>("ModuleId");

                    b.Property<int>("PageTemplateId")
                        .HasColumnName("PageTemplateID");

                    b.HasKey("PlaceId")
                        .HasName("PK_Kastra_Places");

                    b.HasIndex("ModuleId")
                        .IsUnique()
                        .HasFilter("[ModuleId] IS NOT NULL");

                    b.HasIndex("PageTemplateId");

                    b.ToTable("Kastra_Places");
                });

            modelBuilder.Entity("Kastra.DAL.EntityFramework.Models.KastraVisitors", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<string>("IpAddress")
                        .HasColumnName("IpAddress")
                        .HasMaxLength(45);

                    b.Property<DateTime>("LastVisitAt")
                        .HasColumnName("LastVisitAt");

                    b.Property<string>("UserAgent")
                        .HasColumnName("UserAgent")
                        .HasMaxLength(256);

                    b.Property<string>("UserId")
                        .HasColumnName("UserId");

                    b.HasKey("Id")
                        .HasName("PK_Kastra_Visitors");

                    b.ToTable("Kastra_Visitors");
                });

            modelBuilder.Entity("Kastra.DAL.EntityFramework.Models.KastraModuleControls", b =>
                {
                    b.HasOne("Kastra.DAL.EntityFramework.Models.KastraModuleDefinitions", "ModuleDef")
                        .WithMany("KastraModuleControls")
                        .HasForeignKey("ModuleDefId")
                        .HasConstraintName("FK_Kastra_Module_Controls_Kastra_Modules")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Kastra.DAL.EntityFramework.Models.KastraModulePermissions", b =>
                {
                    b.HasOne("Kastra.DAL.EntityFramework.Models.KastraModules", "Module")
                        .WithMany("KastraModulePermissions")
                        .HasForeignKey("ModuleId")
                        .HasConstraintName("FK_Kastra_Module_Permissions_Kastra_Modules")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Kastra.DAL.EntityFramework.Models.KastraPermissions", "Permission")
                        .WithMany("KastraModulePermissions")
                        .HasForeignKey("PermissionId")
                        .HasConstraintName("FK_Kastra_Module_Permissions_Kastra_Module_Permissions")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Kastra.DAL.EntityFramework.Models.KastraModules", b =>
                {
                    b.HasOne("Kastra.DAL.EntityFramework.Models.KastraModuleDefinitions", "ModuleDef")
                        .WithMany("KastraModules")
                        .HasForeignKey("ModuleDefId")
                        .HasConstraintName("FK_Kastra_Modules_Kastra_Module_Definitions")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Kastra.DAL.EntityFramework.Models.KastraPlaces", "Place")
                        .WithMany("KastraModules")
                        .HasForeignKey("PlaceId")
                        .HasConstraintName("FK_Kastra_Modules_Kastra_Places")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Kastra.DAL.EntityFramework.Models.KastraPages", b =>
                {
                    b.HasOne("Kastra.DAL.EntityFramework.Models.KastraPageTemplates", "PageTemplate")
                        .WithMany("KastraPages")
                        .HasForeignKey("PageTemplateId")
                        .HasConstraintName("FK_Kastra_Pages_Kastra_Page_Templates")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Kastra.DAL.EntityFramework.Models.KastraPlaces", b =>
                {
                    b.HasOne("Kastra.DAL.EntityFramework.Models.KastraModules", "StaticKastraModule")
                        .WithOne("StaticPlace")
                        .HasForeignKey("Kastra.DAL.EntityFramework.Models.KastraPlaces", "ModuleId");

                    b.HasOne("Kastra.DAL.EntityFramework.Models.KastraPageTemplates", "PageTemplate")
                        .WithMany("KastraPlaces")
                        .HasForeignKey("PageTemplateId")
                        .HasConstraintName("FK_Kastra_Places_Kastra_Page_Templates")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
