using Microsoft.EntityFrameworkCore;
using Kastra.DAL.EntityFramework.Models;
using Kastra.Core.Identity;

namespace Kastra.DAL.EntityFramework
{
    public class KastraDbContext : ApplicationDbContext
    {
        public virtual DbSet<MailTemplate> KastraMailTemplates { get; set; }
        public virtual DbSet<ModuleControl> KastraModuleControls { get; set; }
        public virtual DbSet<ModuleDefinition> KastraModuleDefinitions { get; set; }
        public virtual DbSet<ModulePermission> KastraModulePermissions { get; set; }
        public virtual DbSet<ModuleNavigation> KastraModuleNavigations { get; set; }
        public virtual DbSet<Module> KastraModules { get; set; }
        public virtual DbSet<PageTemplate> KastraPageTemplates { get; set; }
        public virtual DbSet<Page> KastraPages { get; set; }
        public virtual DbSet<Parameter> KastraParameters { get; set; }
        public virtual DbSet<Permission> KastraPermissions { get; set; }
        public virtual DbSet<Place> KastraPlaces { get; set; }
        public virtual DbSet<Visitor> KastraVisitors { get; set; }
        public virtual DbSet<File> KastraFiles { get; set; }

        
        public KastraDbContext(DbContextOptions<KastraDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Visitor>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK_Kastra_Visitors");

                entity.ToTable("Kastra_Visitors");

                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.IpAddress).HasColumnName("IpAddress").HasMaxLength(45);

                entity.Property(e => e.LastVisitAt).HasColumnName("LastVisitAt");

                entity.Property(e => e.UserAgent).HasColumnName("UserAgent").HasMaxLength(256);

                entity.Property(e => e.UserId).HasColumnName("UserId");

                entity.HasOne(e => e.User).WithMany().HasForeignKey(e => e.UserId);
            });

            modelBuilder.Entity<ModuleControl>(entity =>
            {
                entity.HasKey(e => e.ModuleControlId)
                    .HasName("PK_Kastra_Module_Controls");

                entity.ToTable("Kastra_Module_Controls");

                entity.Property(e => e.ModuleControlId).HasColumnName("ModuleControlID");

                entity.Property(e => e.KeyName).HasMaxLength(150);

                entity.Property(e => e.ModuleDefinitionId).HasColumnName("ModuleDefID");

                entity.Property(e => e.Path).HasMaxLength(250);

                entity.HasOne(d => d.ModuleDefinition)
                    .WithMany(p => p.ModuleControls)
                    .HasForeignKey(d => d.ModuleDefinitionId)
                    .HasConstraintName("FK_Kastra_Module_Controls_Kastra_Modules");
            });

            modelBuilder.Entity<ModuleDefinition>(entity =>
            {
                entity.HasKey(e => e.ModuleDefId)
                    .HasName("PK_Kastra_Module_Definitions");

                entity.ToTable("Kastra_Module_Definitions");

                entity.Property(e => e.ModuleDefId).HasColumnName("ModuleDefID");

                entity.Property(e => e.KeyName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Path)
                    .IsRequired()
                    .HasMaxLength(200);

				entity.Property(e => e.Namespace)
					.HasMaxLength(150);

                entity.Property(e => e.Version)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ModulePermission>(entity =>
            {
                entity.HasKey(e => e.ModulePermissionId)
                    .HasName("PK_Kastra_Module_Permissions");

                entity.ToTable("Kastra_Module_Permissions");

                entity.Property(e => e.ModulePermissionId).HasColumnName("ModulePermissionID");

                entity.Property(e => e.ModuleId).HasColumnName("ModuleID");

                entity.Property(e => e.PermissionId).HasColumnName("PermissionID");

                entity.HasOne(d => d.Module)
				    .WithMany(p => p.ModulePermissions)
                	.HasForeignKey(d => d.ModuleId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Kastra_Module_Permissions_Kastra_Modules");

                entity.HasOne(d => d.Permission)
                    .WithMany(p => p.KastraModulePermissions)
                    .HasForeignKey(d => d.PermissionId)
                    .HasConstraintName("FK_Kastra_Module_Permissions_Kastra_Module_Permissions");
            });

            modelBuilder.Entity<ModuleNavigation>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK_Kastra_Module_Navigations");

                entity.ToTable("Kastra_Module_Navigations");

                entity.Property(e => e.Id).HasColumnName("ModuleNavigationID");

                entity.Property(e => e.ModuleDefinitionId).HasColumnName("ModuleDefinitionID");

                entity.Property(e => e.Name);

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasMaxLength(2000);
                
                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.HasOne(d => d.ModuleDefinition)
                    .WithMany(p => p.ModuleNavigations)
                    .HasForeignKey(d => d.ModuleDefinitionId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Kastra_Module_Navigations_Kastra_Module!Definitions");
            });

            modelBuilder.Entity<Module>(entity =>
            {
                entity.HasKey(e => e.ModuleId)
                    .HasName("PK_Kastra_Modules");

                entity.ToTable("Kastra_Modules");

                entity.Property(e => e.ModuleId).HasColumnName("ModuleID");

                entity.Property(e => e.ModuleDefinitionId).HasColumnName("ModuleDefID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.PageId).HasColumnName("PageID");

                entity.Property(e => e.PlaceId).HasColumnName("PlaceID");

                entity.Property(e => e.IsDisabled).HasColumnName("IsDisabled");

                entity.HasOne(d => d.ModuleDefinition)
                    .WithMany(p => p.Modules)
                    .HasForeignKey(d => d.ModuleDefinitionId)
                    .HasConstraintName("FK_Kastra_Modules_Kastra_Module_Definitions");

                entity.HasOne(d => d.Place)
                    .WithMany(p => p.KastraModules)
                    .HasForeignKey(d => d.PlaceId)
                    .HasConstraintName("FK_Kastra_Modules_Kastra_Places");

                entity.HasOne(d => d.StaticPlace)
                    .WithOne(p => p.StaticKastraModule)
                    .HasForeignKey<Place>(p => p.ModuleId)
                    .IsRequired(false);
            });

            modelBuilder.Entity<PageTemplate>(entity =>
            {
                entity.HasKey(e => e.PageTemplateId)
                    .HasName("PK_Kastra_Page_Templates");

                entity.ToTable("Kastra_Page_Templates");

                entity.Property(e => e.PageTemplateId).HasColumnName("PageTemplateID");

                entity.Property(e => e.KeyName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.ModelClass)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.ViewPath)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<Page>(entity =>
            {
                entity.HasKey(e => e.PageId)
                    .HasName("PK_Kastra_Pages");

                entity.ToTable("Kastra_Pages");

                entity.Property(e => e.PageId).HasColumnName("PageID");

                entity.Property(e => e.KeyName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PageTemplateId).HasColumnName("PageTemplateID");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(150);

				entity.Property(e => e.MetaDescription)
					.HasMaxLength(160);

				entity.Property(e => e.MetaKeywords)
					.HasMaxLength(250);

				entity.Property(e => e.MetaRobot)
					.HasMaxLength(50);

                entity.HasOne(d => d.PageTemplate)
                    .WithMany(p => p.KastraPages)
                    .HasForeignKey(d => d.PageTemplateId)
                    .HasConstraintName("FK_Kastra_Pages_Kastra_Page_Templates");
            });

            modelBuilder.Entity<Parameter>(entity =>
            {
                entity.HasKey(e => e.ParameterId)
                    .HasName("PK_Kastra_Parameters");

                entity.ToTable("Kastra_Parameters");

                entity.Property(e => e.Key).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(150);

                entity.Property(e => e.Value).HasMaxLength(1000);
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.HasKey(e => e.PermissionId)
                    .HasName("PK_Kastra_Permissions");

                entity.ToTable("Kastra_Permissions");

                entity.Property(e => e.PermissionId).HasColumnName("PermissionID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<Place>(entity =>
            {
                entity.HasKey(e => e.PlaceId)
                    .HasName("PK_Kastra_Places");

                entity.ToTable("Kastra_Places");

                entity.Property(e => e.PlaceId).HasColumnName("PlaceID");

                entity.Property(e => e.KeyName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PageTemplateId).HasColumnName("PageTemplateID");

                entity.HasOne(d => d.PageTemplate)
                    .WithMany(p => p.KastraPlaces)
                    .HasForeignKey(d => d.PageTemplateId)
                    .HasConstraintName("FK_Kastra_Places_Kastra_Page_Templates");

                entity.Property(e => e.ModuleId);
            });

            modelBuilder.Entity<File>(entity => {
                entity.HasKey(e => e.FileId)
                    .HasName("PK_Kastra_Files");

                entity.ToTable("Kastra_Files");

                entity.Property(e => e.FileId)
                    .HasColumnName("FileID");

                entity.Property(e => e.Name)
                    .HasMaxLength(150);

                entity.Property(e => e.Path)
                    .HasMaxLength(500);

                entity.Property(e => e.DateCreated)
                    .HasDefaultValueSql("getdate()");
            });

            modelBuilder.Entity<MailTemplate>(entity =>
            {
                entity.HasKey(e => e.MailTemplateId)
                    .HasName("PK_Kastra_Mail_Templates");

                entity.ToTable("Kastra_Mail_Templates");

                entity.Property(e => e.Keyname)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Subject)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Message);
            });
        }
    }
}