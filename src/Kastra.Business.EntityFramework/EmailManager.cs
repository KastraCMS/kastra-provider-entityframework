using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kastra.Business.Mappers;
using Kastra.Core.Business;
using Kastra.Core.Dto;
using Kastra.Core.Services;
using Kastra.DAL.EntityFramework;
using Kastra.DAL.EntityFramework.Models;

namespace Kastra.Business
{
    public class EmailManager : IEmailManager
	{
		private readonly KastraContext _dbContext;
		private readonly CacheEngine _cacheEngine;
        private readonly IEmailSender _emailSender;

        public EmailManager(KastraContext dbContext, CacheEngine cacheEngine, IEmailSender emailSender)
		{
            _cacheEngine = cacheEngine;
			_dbContext = dbContext;
            _emailSender = emailSender;
		}

        public void AddMailTemplate(string keyname, string subject, string message)
        {
            if (string.IsNullOrEmpty(keyname))
            {
                throw new ArgumentNullException(nameof(keyname));
            }

            if (_dbContext.KastraMailTemplates.Any(mt => mt.Keyname == keyname))
            {
                throw new ArgumentException("The keyname already exists");
            }

            KastraMailTemplate mailTemplate = new KastraMailTemplate()
            {
                Keyname = keyname,
                Subject = subject,
                Message = message
            };

            _dbContext.KastraMailTemplates.Add(mailTemplate);
            _dbContext.SaveChanges();

            _cacheEngine.ClearCacheContains("Mail_Template");
        }

        public void DeleteMailTemplate(string keyname)
        {
            if (string.IsNullOrEmpty(keyname))
            {
                throw new ArgumentNullException(nameof(keyname));
            }

            KastraMailTemplate mailTemplate = _dbContext.KastraMailTemplates.SingleOrDefault(mt => mt.Keyname == keyname);

            if (mailTemplate == null)
            {
                throw new ArgumentException("The mail template was not found");
            }

            _dbContext.KastraMailTemplates.Remove(mailTemplate);
            _dbContext.SaveChanges();

            ClearMailTemplateCache();
        }

        public string Format(string template, Dictionary<string, string> data)
        {
            if (string.IsNullOrEmpty(template))
            {
                throw new ArgumentNullException(nameof(template));
            }

            if (data == null)
            {
                return template;
            }

            foreach (KeyValuePair<string,string> keyValue in data)
            {
                template = template.Replace($"{{{keyValue.Key}}}", keyValue.Value);
            }

            return template;
        }

        public MailTemplateInfo GetMailTemplate(string keyname)
        {
            if (string.IsNullOrEmpty(keyname))
            {
                throw new ArgumentNullException(nameof(keyname));
            }

            KastraMailTemplate mailTemplate = _dbContext.KastraMailTemplates.SingleOrDefault(mt => mt.Keyname == keyname);

            if (mailTemplate == null)
            {
                return null;
            }

            return mailTemplate.ToMailTemplateInfo();
        }

        public IList<MailTemplateInfo> GetMailTemplates()
        {
            List<MailTemplateInfo> mailTemplates = _dbContext.KastraMailTemplates.Select(mt => mt.ToMailTemplateInfo()).ToList();

            if (!_cacheEngine.GetCacheObject("Mail_Templates", out mailTemplates))
            {
                mailTemplates = _dbContext.KastraMailTemplates.Select(mt => mt.ToMailTemplateInfo()).ToList();
            }
            
            return mailTemplates;
        }

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

            KastraMailTemplate mailTemplate = _dbContext.KastraMailTemplates.SingleOrDefault(mt => mt.Keyname == templateName);

            if (mailTemplate == null)
            {
                throw new NullReferenceException($"{nameof(mailTemplate)} not found.");
            }

            string subject = Format(mailTemplate.Subject, data);
            string message = Format(mailTemplate.Message, data);

            _emailSender.SendEmail(email, subject, message);
        }

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

            KastraMailTemplate mailTemplate = _dbContext.KastraMailTemplates.SingleOrDefault(mt => mt.Keyname == templateName);

            if (mailTemplate == null)
            {
                throw new NullReferenceException($"{nameof(mailTemplate)} not found.");
            }

            string subject = Format(mailTemplate.Subject, data);
            string message = Format(mailTemplate.Message, data);
            
            await _emailSender.SendEmailAsync(email, subject, message);
        }

        public void UpdateMailTemplate(MailTemplateInfo mailTemplateInfo)
        {
            if (mailTemplateInfo is null)
            {
                throw new ArgumentNullException(nameof(mailTemplateInfo));
            }

            KastraMailTemplate mailTemplate = _dbContext.KastraMailTemplates
                                                .SingleOrDefault(mt => mt.Keyname == mailTemplateInfo.Keyname);

            if (mailTemplate == null)
            {
                throw new ArgumentException("The mail template was not found");
            }

            mailTemplate.Subject = mailTemplateInfo.Subject;
            mailTemplate.Message = mailTemplateInfo.Message;

            _dbContext.KastraMailTemplates.Update(mailTemplate);
            _dbContext.SaveChanges();

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
