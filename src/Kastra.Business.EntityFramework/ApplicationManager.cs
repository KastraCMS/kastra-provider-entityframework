using System.Collections.Generic;
using System.Linq;
using Kastra.Core.Business;
using Kastra.Core.Constants;
using Kastra.DAL.EntityFramework;
using Kastra.DAL.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace Kastra.Business
{
    public class ApplicationManager : IApplicationManager
    {
        private readonly KastraContext _dbContext;

        public ApplicationManager(KastraContext dbContext)
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
            KastraPages home = _dbContext.KastraPages.SingleOrDefault(p => p.KeyName.ToLower() == "home");
            KastraPageTemplates template = _dbContext.KastraPageTemplates
                                                .SingleOrDefault(t => t.KeyName == SiteConfiguration.DefaultPageTemplateKeyName);

            if(home == null && template != null)
            {
                home = new KastraPages();
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
            KastraPlaces place = null;

            // Install default template
            KastraPageTemplates template = _dbContext.KastraPageTemplates
                                                .SingleOrDefault(t => t.KeyName == SiteConfiguration.DefaultPageTemplateKeyName);

            if (template != null)
            {
                return;
            }

            template = new KastraPageTemplates();
            template.KeyName = SiteConfiguration.DefaultPageTemplateKeyName;
            template.Name = "Default template";
            template.ModelClass = "Kastra.Web.Models.Template.DefaultTemplateViewModel";
            template.ViewPath = "Page";

            template.KastraPlaces = new List<KastraPlaces>();

            // Add places
            place = new KastraPlaces();
            place.KeyName = "Header";
            
            template.KastraPlaces.Add(place);

            place = new KastraPlaces();
            place.KeyName = "Body";
            
            template.KastraPlaces.Add(place);

            place = new KastraPlaces();
            place.KeyName = "Footer";
            
            template.KastraPlaces.Add(place);
            
            _dbContext.KastraPageTemplates.Add(template);

            // Add home template
            KastraPageTemplates homeTemplate = _dbContext.KastraPageTemplates
                                                .SingleOrDefault(t => t.KeyName == SiteConfiguration.DefaultPageTemplateKeyName);

            if (homeTemplate != null)
            {
                return;
            }

            homeTemplate = new KastraPageTemplates();
            homeTemplate.KeyName = "HomeTemplate";
            homeTemplate.Name = "Home template";
            homeTemplate.ModelClass = "Kastra.Web.Models.Template.HomeTemplateViewModel";
            homeTemplate.ViewPath = "Page";

            homeTemplate.KastraPlaces = new List<KastraPlaces>();

            // Add places
            place = new KastraPlaces();
            place.KeyName = "Header";
            
            homeTemplate.KastraPlaces.Add(place);

            place = new KastraPlaces();
            place.KeyName = "Body";
            
            homeTemplate.KastraPlaces.Add(place);

            place = new KastraPlaces();
            place.KeyName = "Footer";
            
            homeTemplate.KastraPlaces.Add(place);
            
            _dbContext.KastraPageTemplates.Add(homeTemplate);

            _dbContext.SaveChanges();
        }

        public void InstallDefaultPermissions()
        {
            // Granted permission
            KastraPermissions permission = new KastraPermissions();
            permission.Name = ModuleConfiguration.GrantedAccessPermission;
            _dbContext.KastraPermissions.Add(permission);

            // Read
            permission = new KastraPermissions();
            permission.Name = ModuleConfiguration.ReadPermission;
            _dbContext.KastraPermissions.Add(permission);

            // Edit
            permission = new KastraPermissions();
            permission.Name = ModuleConfiguration.EditPermission;
            _dbContext.KastraPermissions.Add(permission);

            _dbContext.SaveChanges();
        }

        public void InstallDefaultParameters()
        {
            KastraParameters theme = _dbContext.KastraParameters.SingleOrDefault(p => p.Key == "Theme");

            if(theme == null)
            {
                theme = new KastraParameters();
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

            KastraMailTemplate mailTemplate = null;
            List<KastraMailTemplate> mailTemplates = new List<KastraMailTemplate>();

            if (!_dbContext.KastraMailTemplates.Any(mt => mt.Keyname == "account.confirmregistration"))
            {
                mailTemplate = new KastraMailTemplate() {
                    Keyname = "account.confirmregistration",
                    Subject = "Confirm your account",
                    Message = "Please confirm your account by clicking this link: <a href='{callbackUrl}'>link</a>"
                };

                mailTemplates.Add(mailTemplate);
            }
            
            if (!_dbContext.KastraMailTemplates.Any(mt => mt.Keyname == "account.resetpassword"))
            {
                mailTemplate = new KastraMailTemplate() {
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