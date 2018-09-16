using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Kastra.Core.Business;
using Kastra.Core.Dto;
using Kastra.Core.Services;

namespace Kastra.Business
{
    public class EmailSender : IEmailSender
    {
        private readonly SmtpClient _smtpClient;
        private readonly IParameterManager _parameterManager;

        public EmailSender(IParameterManager parameterManager)
        {
            _parameterManager = parameterManager;

            // Get site configuration
            SiteConfigurationInfo siteConfiguration = parameterManager.GetSiteConfiguration();

            // Set smtp client
            _smtpClient = new SmtpClient
            {
                Host = siteConfiguration.SmtpHost,
                Port = siteConfiguration.SmtpPort,
                EnableSsl = siteConfiguration.SmtpEnableSsl
            };

            if (!String.IsNullOrEmpty(siteConfiguration.SmtpCredentialsUser))
            {
                _smtpClient.Credentials = new NetworkCredential(siteConfiguration.SmtpCredentialsUser, 
                                                                siteConfiguration.SmtpCredentialsPassword);
            }
        }

        /// <summary>
        /// Sends the email.
        /// </summary>
        /// <param name="email">Email.</param>
        /// <param name="subject">Subject.</param>
        /// <param name="message">Message.</param>
        public void SendEmail(string email, string subject, string message)
        {
            SiteConfigurationInfo siteConfiguration = _parameterManager.GetSiteConfiguration();

            using (MailMessage mailMessage = new MailMessage(siteConfiguration.EmailSender, email))
            {
                mailMessage.Subject = subject;
                mailMessage.Body = message;

                _smtpClient.Send(mailMessage);
            }
        }

        /// <summary>
        /// Sends the email async.
        /// </summary>
        /// <returns>The email async.</returns>
        /// <param name="email">Email.</param>
        /// <param name="subject">Subject.</param>
        /// <param name="message">Message.</param>
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            SiteConfigurationInfo siteConfiguration = _parameterManager.GetSiteConfiguration();

            using (MailMessage mailMessage = new MailMessage(siteConfiguration.EmailSender, email))
            {
                mailMessage.Subject = subject;
                mailMessage.Body = message;

                await _smtpClient.SendMailAsync(mailMessage);
            }
        }
    }
}
