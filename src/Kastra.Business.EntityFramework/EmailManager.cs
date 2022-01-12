using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kastra.Business.Mappers;
using Kastra.Core.Services.Contracts;
using Kastra.Core.DTO;
using Kastra.Core.Services;
using Kastra.DAL.EntityFramework;
using Kastra.DAL.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace Kastra.Business
{
    public class EmailManager : IEmailManager
	{
		private readonly KastraDbContext _dbContext;
		private readonly CacheEngine _cacheEngine;
        private readonly IEmailSender _emailSender;

        public EmailManager(KastraDbContext dbContext, CacheEngine cacheEngine, IEmailSender emailSender)
		{
            _cacheEngine = cacheEngine;
			_dbContext = dbContext;
            _emailSender = emailSender;
		}

        /// <inheritdoc cref="IEmailManager.AddMailTemplateAsync(string, string, string)" />
        public async Task AddMailTemplateAsync(string keyname, string subject, string message)
        {
            if (string.IsNullOrEmpty(keyname))
            {
                throw new ArgumentNullException(nameof(keyname));
            }

            if (await _dbContext.KastraMailTemplates.AnyAsync(mt => mt.Keyname == keyname))
            {
                throw new ArgumentException("The keyname already exists");
            }

            MailTemplate mailTemplate = new ()
            {
                Keyname = keyname,
                Subject = subject,
                Message = message
            };

            _dbContext.KastraMailTemplates.Add(mailTemplate);

            await _dbContext.SaveChangesAsync();

            _cacheEngine.ClearCacheContains("Mail_Template");
        }

        /// <inheritdoc cref="IEmailManager.DeleteMailTemplateAsync(string)" />
        public async Task DeleteMailTemplateAsync(string keyname)
        {
            if (string.IsNullOrEmpty(keyname))
            {
                throw new ArgumentNullException(nameof(keyname));
            }

            MailTemplate mailTemplate = await _dbContext.KastraMailTemplates.SingleOrDefaultAsync(mt => mt.Keyname == keyname);

            if (mailTemplate is null)
            {
                throw new ArgumentException("The mail template was not found");
            }

            _dbContext.KastraMailTemplates.Remove(mailTemplate);
            await _dbContext.SaveChangesAsync();

            ClearMailTemplateCache();
        }

        /// <inheritdoc cref="IEmailManager.Format(string, Dictionary{string, string})"/>
        public string Format(string template, Dictionary<string, string> data)
        {
            if (string.IsNullOrEmpty(template))
            {
                throw new ArgumentNullException(nameof(template));
            }

            if (data is null)
            {
                return template;
            }

            foreach (KeyValuePair<string,string> keyValue in data)
            {
                template = template.Replace($"{{{keyValue.Key}}}", keyValue.Value);
            }

            return template;
        }

        /// <inheritdoc cref="IEmailManager.GetMailTemplateAsync(string)" />
        public async Task<MailTemplateInfo> GetMailTemplateAsync(string keyname)
        {
            if (string.IsNullOrEmpty(keyname))
            {
                throw new ArgumentNullException(nameof(keyname));
            }

            MailTemplate mailTemplate = await _dbContext.KastraMailTemplates.SingleOrDefaultAsync(mt => mt.Keyname == keyname);

            return mailTemplate.ToMailTemplateInfo();
        }

        /// <inheritdoc cref="IEmailManager.GetMailTemplatesAsync"/>
        public async Task<IList<MailTemplateInfo>> GetMailTemplatesAsync()
        {
            List<MailTemplateInfo> mailTemplates = _dbContext.KastraMailTemplates.Select(mt => mt.ToMailTemplateInfo()).ToList();

            if (!_cacheEngine.GetCacheObject("Mail_Templates", out mailTemplates))
            {
                mailTemplates = await _dbContext.KastraMailTemplates.Select(mt => mt.ToMailTemplateInfo()).ToListAsync();
            }
            
            return mailTemplates;
        }

        /// <inheritdoc cref="IEmailManager.SendEmail(string, string, Dictionary{string, string})" />
        public void SendEmail(string email, string templateName, Dictionary<string, string> data)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            if (string.IsNullOrEmpty(templateName))
            {
                throw new ArgumentNullException(nameof(templateName));
            }

            MailTemplate mailTemplate = _dbContext.KastraMailTemplates.SingleOrDefault(mt => mt.Keyname == templateName);

            if (mailTemplate == null)
            {
                throw new NullReferenceException($"{nameof(mailTemplate)} not found.");
            }

            string subject = Format(mailTemplate.Subject, data);
            string message = Format(mailTemplate.Message, data);

            _emailSender.SendEmail(email, subject, message);
        }

        /// <inheritdoc cref="IEmailManager.SendEmailAsync(string, string, Dictionary{string, string})" />
        public async Task SendEmailAsync(string email, string templateName, Dictionary<string, string> data)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            if (string.IsNullOrEmpty(templateName))
            {
                throw new ArgumentNullException(nameof(templateName));
            }

            MailTemplate mailTemplate = await _dbContext.KastraMailTemplates.SingleOrDefaultAsync(mt => mt.Keyname == templateName);

            if (mailTemplate is null)
            {
                throw new NullReferenceException($"{nameof(mailTemplate)} not found.");
            }

            string subject = Format(mailTemplate.Subject, data);
            string message = Format(mailTemplate.Message, data);
            
            await _emailSender.SendEmailAsync(email, subject, message);
        }

        /// <inheritdoc cref="IEmailManager.UpdateMailTemplateAsync(MailTemplateInfo)" />
        public async Task UpdateMailTemplateAsync(MailTemplateInfo mailTemplateInfo)
        {
            if (mailTemplateInfo is null)
            {
                throw new ArgumentNullException(nameof(mailTemplateInfo));
            }

            MailTemplate mailTemplate = await _dbContext.KastraMailTemplates
                                                .SingleOrDefaultAsync(mt => mt.Keyname == mailTemplateInfo.Keyname);

            if (mailTemplate == null)
            {
                throw new ArgumentException("The mail template was not found");
            }

            mailTemplate.Subject = mailTemplateInfo.Subject;
            mailTemplate.Message = mailTemplateInfo.Message;

            _dbContext.KastraMailTemplates.Update(mailTemplate);

            await _dbContext.SaveChangesAsync();

            ClearMailTemplateCache();
        }

        /// <summary>
        /// Clear all mail templates in cache.
        /// </summary>
        private void ClearMailTemplateCache()
        {
            _cacheEngine.ClearCacheContains("Mail_Template");
        }
    }
}
