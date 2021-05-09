using System.Collections.Generic;
using System.Linq;
using Kastra.Core.Services.Contracts;
using Kastra.Core.Constants;
using Kastra.DAL.EntityFramework;
using Kastra.DAL.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace Kastra.Business
{
    public class ApplicationManager : IApplicationManager
    {
        private readonly KastraDbContext _dbContext;

        public ApplicationManager(KastraDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Install()
        {
            // Create database
            _dbContext.Database.Migrate();

            InstallDefaultParameters();

            InstallDefaultMailTemplates();
        }

        public void InstallDefaultPage()
        {
            Page home = _dbContext.KastraPages.SingleOrDefault(p => p.KeyName.ToLower() == "home");
            PageTemplate template = _dbContext.KastraPageTemplates
                                                .SingleOrDefault(t => t.KeyName == SiteConfiguration.DefaultPageTemplateKeyName);

            if(home == null && template != null)
            {
                home = new Page();
                home.KeyName = "home";
                home.MetaDescription = "My home page";
                home.PageTemplateId = template.PageTemplateId;
                home.Title = "Home";
                home.MetaKeywords = string.Empty;
                home.MetaDescription = string.Empty;
                home.MetaRobot = string.Empty;

                _dbContext.KastraPages.Add(home);
                _dbContext.SaveChanges();
            }
        }

        public void InstallDefaultTemplate()
        {
            Place place = null;

            // Install default template
            PageTemplate template = _dbContext.KastraPageTemplates
                                                .SingleOrDefault(t => t.KeyName == SiteConfiguration.DefaultPageTemplateKeyName);

            if (template != null)
            {
                return;
            }

            template = new PageTemplate();
            template.KeyName = SiteConfiguration.DefaultPageTemplateKeyName;
            template.Name = "Default template";
            template.ModelClass = "Kastra.Server.Models.Template.DefaultTemplateViewModel";
            template.ViewPath = "Page";

            template.KastraPlaces = new List<Place>();

            // Add places
            place = new Place();
            place.KeyName = "Header";
            
            template.KastraPlaces.Add(place);

            place = new Place();
            place.KeyName = "Body";
            
            template.KastraPlaces.Add(place);

            place = new Place();
            place.KeyName = "Footer";
            
            template.KastraPlaces.Add(place);
            
            _dbContext.KastraPageTemplates.Add(template);

            // Add home template
            PageTemplate homeTemplate = _dbContext.KastraPageTemplates
                                                .SingleOrDefault(t => t.KeyName == SiteConfiguration.DefaultPageTemplateKeyName);

            if (homeTemplate != null)
            {
                return;
            }

            homeTemplate = new PageTemplate();
            homeTemplate.KeyName = "HomeTemplate";
            homeTemplate.Name = "Home template";
            homeTemplate.ModelClass = "Kastra.Server.Models.Template.HomeTemplateViewModel";
            homeTemplate.ViewPath = "Page";

            homeTemplate.KastraPlaces = new List<Place>();

            // Add places
            place = new Place();
            place.KeyName = "Header";
            
            homeTemplate.KastraPlaces.Add(place);

            place = new Place();
            place.KeyName = "Body";
            
            homeTemplate.KastraPlaces.Add(place);

            place = new Place();
            place.KeyName = "Footer";
            
            homeTemplate.KastraPlaces.Add(place);
            
            _dbContext.KastraPageTemplates.Add(homeTemplate);

            _dbContext.SaveChanges();
        }

        public void InstallDefaultPermissions()
        {
            // Granted permission
            Permission permission = new Permission();
            permission.Name = ModuleConfiguration.GrantedAccessPermission;
            _dbContext.KastraPermissions.Add(permission);

            // Read
            permission = new Permission();
            permission.Name = ModuleConfiguration.ReadPermission;
            _dbContext.KastraPermissions.Add(permission);

            // Edit
            permission = new Permission();
            permission.Name = ModuleConfiguration.EditPermission;
            _dbContext.KastraPermissions.Add(permission);

            _dbContext.SaveChanges();
        }

        public void InstallDefaultParameters()
        {
            Parameter theme = _dbContext.KastraParameters.SingleOrDefault(p => p.Key == "Theme");

            if(theme == null)
            {
                theme = new Parameter();
                theme.Key = "Theme";
                theme.Value = "default";
                _dbContext.Add(theme);
            }
            else
            {
                theme.Value = "default";
                _dbContext.Update(theme);
            }

            _dbContext.SaveChanges();
        }

        public void InstallDefaultMailTemplates()
        {
            if (_dbContext.KastraMailTemplates.Any())
            {
                return;
            }

            MailTemplate mailTemplate = null;
            List<MailTemplate> mailTemplates = new List<MailTemplate>();

            if (!_dbContext.KastraMailTemplates.Any(mt => mt.Keyname == "account.confirmregistration"))
            {
                mailTemplate = new MailTemplate() {
                    Keyname = "account.confirmregistration",
                    Subject = "Confirm your account",
                    Message = "Please confirm your account by clicking this link: <a href='{callbackUrl}'>link</a>"
                };

                mailTemplates.Add(mailTemplate);
            }
            
            if (!_dbContext.KastraMailTemplates.Any(mt => mt.Keyname == "account.resetpassword"))
            {
                mailTemplate = new MailTemplate() {
                    Keyname = "account.resetpassword",
                    Subject = "Reset Password",
                    Message = "Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>"
                };

                mailTemplates.Add(mailTemplate);
            }

            _dbContext.SaveChanges();
        }
    }
}