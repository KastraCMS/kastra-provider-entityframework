using System;
using System.Collections.Generic;
using System.Linq;
using Kastra.Core;
using Kastra.Core.Business;
using Kastra.DAL.EntityFramework;
using Kastra.DAL.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace Kastra.Business
{
    public class ApplicationManager : IApplicationManager
    {
        private readonly KastraContext _dbContext = null;

        public ApplicationManager(KastraContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Install()
        {
            // Create database
            _dbContext.Database.Migrate();

            // Set default parameters
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

        public void InstallDefaultPage()
        {
            KastraPages home = _dbContext.KastraPages.SingleOrDefault(p => p.KeyName.ToLower() == "home");
            KastraPageTemplates template = _dbContext.KastraPageTemplates
                                                .SingleOrDefault(t => t.KeyName == Constants.SiteConfig.DefaultPageTemplateKeyName);

            if(home == null && template != null)
            {
                home = new KastraPages();
                home.KeyName = "home";
                home.MetaDescription = "My home page";
                home.PageTemplateId = template.PageTemplateId;
                home.Title = "Home";
                home.MetaKeywords = String.Empty;
                home.MetaDescription = String.Empty;
                home.MetaRobot = String.Empty;

                _dbContext.KastraPages.Add(home);
                _dbContext.SaveChanges();
            }
        }

        public void InstallDefaultTemplate()
        {
            KastraPlaces place = null;

            // Install default template
            KastraPageTemplates template = _dbContext.KastraPageTemplates
                                                .SingleOrDefault(t => t.KeyName == Constants.SiteConfig.DefaultPageTemplateKeyName);
            
            if(template != null)
                return;

            template = new KastraPageTemplates();
            template.KeyName = Constants.SiteConfig.DefaultPageTemplateKeyName;
            template.Name = "Default template";
            template.Path = "Kastra.Web.Models.Template";

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
                                                .SingleOrDefault(t => t.KeyName == Constants.SiteConfig.DefaultPageTemplateKeyName);
            
            if(homeTemplate != null)
                return;

            homeTemplate = new KastraPageTemplates();
            homeTemplate.KeyName = "HomeTemplate";
            homeTemplate.Name = "Home template";
            homeTemplate.Path = "Kastra.Web.Models.Template";

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
            permission.Name = Constants.ModuleConfig.GrantedAccessPermission;
            _dbContext.KastraPermissions.Add(permission);

            // Read
            permission = new KastraPermissions();
            permission.Name = Constants.ModuleConfig.ReadPermission;
            _dbContext.KastraPermissions.Add(permission);

            // Edit
            permission = new KastraPermissions();
            permission.Name = Constants.ModuleConfig.EditPermission;
            _dbContext.KastraPermissions.Add(permission);

            _dbContext.SaveChanges();
        }
    }
}