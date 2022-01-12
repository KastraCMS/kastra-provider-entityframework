using System.Collections.Generic;
using System.Linq;
using Kastra.Core.Services.Contracts;
using Kastra.Core.Constants;
using Kastra.DAL.EntityFramework;
using Kastra.DAL.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Kastra.Business
{
    public class ApplicationManager : IApplicationManager
    {
        private readonly KastraDbContext _dbContext;

        public ApplicationManager(KastraDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <inheritdoc cref="IApplicationManager.InstallAsync"/>
        public async Task InstallAsync()
        {
            // Create database
            await _dbContext.Database.MigrateAsync();

            await InstallDefaultParametersAsync();

            await InstallDefaultMailTemplatesAsync();
        }

        /// <inheritdoc cref="IApplicationManager.InstallDefaultPageAsync" />
        public async Task InstallDefaultPageAsync()
        {
            Page home = await _dbContext.KastraPages.SingleOrDefaultAsync(p => p.KeyName.ToLower() == "home");
            PageTemplate template = await _dbContext.KastraPageTemplates
                                                .SingleOrDefaultAsync(t => t.KeyName == SiteConfiguration.DefaultPageTemplateKeyName);

            if(home is null && template is not null)
            {
                home = new ()
                {
                    KeyName = "home",
                    PageTemplateId = template.PageTemplateId,
                    Title = "Home",
                    MetaKeywords = string.Empty,
                    MetaDescription = string.Empty,
                    MetaRobot = string.Empty
                };

                _dbContext.KastraPages.Add(home);

                await _dbContext.SaveChangesAsync();
            }
        }

        /// <inheritdoc cref="IApplicationManager.InstallDefaultTemplateAsync" />
        public async Task InstallDefaultTemplateAsync()
        {
            Place place = null;

            // Install default template
            PageTemplate template = _dbContext.KastraPageTemplates
                                                .SingleOrDefault(t => t.KeyName == SiteConfiguration.DefaultPageTemplateKeyName);

            if (template is not null)
            {
                return;
            }

            template = new PageTemplate()
            {
                KeyName = SiteConfiguration.DefaultPageTemplateKeyName,
                Name = "Default template",
                ModelClass = "Kastra.Server.Models.Template.DefaultTemplateViewModel",
                ViewPath = "Page"
            };

            template.KastraPlaces = new List<Place>();

            // Add places
            place = new ()
            {
                KeyName = "Header"
            };
            
            template.KastraPlaces.Add(place);

            place = new ()
            {
                KeyName = "Body"
            };
            
            template.KastraPlaces.Add(place);

            place = new Place()
            {
                KeyName = "Footer"
            };
            
            template.KastraPlaces.Add(place);
            
            _dbContext.KastraPageTemplates.Add(template);

            // Add home template
            PageTemplate homeTemplate = _dbContext.KastraPageTemplates
                                                .SingleOrDefault(t => t.KeyName == SiteConfiguration.DefaultPageTemplateKeyName);

            if (homeTemplate != null)
            {
                return;
            }

            homeTemplate = new ()
            {
                KeyName = "HomeTemplate",
                Name = "Home template",
                ModelClass = "Kastra.Server.Models.Template.HomeTemplateViewModel",
                ViewPath = "Page"
            };

            homeTemplate.KastraPlaces = new List<Place>();

            // Add places
            place = new ()
            {
                KeyName = "Header"
            };
            
            homeTemplate.KastraPlaces.Add(place);

            place = new ()
            {
                KeyName = "Body"
            };
            
            homeTemplate.KastraPlaces.Add(place);

            place = new ()
            {
                KeyName = "Footer"
            };
            
            homeTemplate.KastraPlaces.Add(place);
            
            _dbContext.KastraPageTemplates.Add(homeTemplate);

            await _dbContext.SaveChangesAsync();
        }

        /// <inheritdoc cref="IApplicationManager.InstallDefaultPermissionsAsync" />
        public async Task InstallDefaultPermissionsAsync()
        {
            // Granted permission
            Permission permission = new()
            {
                Name = ModuleConfiguration.GrantedAccessPermission
            };
            _dbContext.KastraPermissions.Add(permission);

            // Read
            permission = new ()
            {
                Name = ModuleConfiguration.ReadPermission
            };
            _dbContext.KastraPermissions.Add(permission);

            // Edit
            permission = new ()
            {
                Name = ModuleConfiguration.EditPermission
            };
            _dbContext.KastraPermissions.Add(permission);

            await _dbContext.SaveChangesAsync();
        }

        /// <inheritdoc cref="IApplicationManager.InstallDefaultParametersAsync" />
        public async Task InstallDefaultParametersAsync()
        {
            Parameter theme = await _dbContext.KastraParameters.SingleOrDefaultAsync(p => p.Key == "Theme");

            if(theme is null)
            {
                theme = new ()
                {
                    Key = "Theme",
                    Value = "default"
                };
                _dbContext.Add(theme);
            }
            else
            {
                theme.Value = "default";
                _dbContext.Update(theme);
            }

            await _dbContext.SaveChangesAsync();
        }

        /// <inheritdoc cref="IApplicationManager.InstallDefaultMailTemplatesAsync" />
        public async Task InstallDefaultMailTemplatesAsync()
        {
            if (await _dbContext.KastraMailTemplates.AnyAsync())
            {
                return;
            }

            MailTemplate mailTemplate = null;
            List<MailTemplate> mailTemplates = new ();

            if (!(await _dbContext.KastraMailTemplates.AnyAsync(mt => mt.Keyname == "account.confirmregistration")))
            {
                mailTemplate = new MailTemplate() {
                    Keyname = "account.confirmregistration",
                    Subject = "Confirm your account",
                    Message = "Please confirm your account by clicking this link: <a href='{callbackUrl}'>link</a>"
                };

                mailTemplates.Add(mailTemplate);
            }
            
            if (!(await _dbContext.KastraMailTemplates.AnyAsync(mt => mt.Keyname == "account.resetpassword")))
            {
                mailTemplate = new MailTemplate() {
                    Keyname = "account.resetpassword",
                    Subject = "Reset Password",
                    Message = "Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>"
                };

                mailTemplates.Add(mailTemplate);
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}