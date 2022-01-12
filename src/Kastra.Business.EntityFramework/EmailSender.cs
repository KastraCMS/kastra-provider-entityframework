using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Kastra.Core.Services.Contracts;
using Kastra.Core.DTO;

namespace Kastra.Business
{
    public class EmailSender : IEmailSender
    {
        private readonly SmtpClient _smtpClient;
        private readonly SiteConfigurationInfo _siteConfiguration;

        public EmailSender(IParameterManager parameterManager)
        {
            // Get site configuration
            _siteConfiguration = parameterManager.GetSiteConfigurationAsync().Result;

            // Set smtp client
            _smtpClient = new SmtpClient
            {
                Host = !String.IsNullOrEmpty(_siteConfiguration.SmtpHost) ? _siteConfiguration.SmtpHost : "localhost",
                Port = _siteConfiguration.SmtpPort > 0 ? _siteConfiguration.SmtpPort : 587,
                EnableSsl = _siteConfiguration.SmtpEnableSsl
            };

            if (!String.IsNullOrEmpty(_siteConfiguration.SmtpCredentialsUser))
            {
                _smtpClient.Credentials = new NetworkCredential(_siteConfiguration.SmtpCredentialsUser, 
                                                                _siteConfiguration.SmtpCredentialsPassword);
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
            using MailMessage mailMessage = new(_siteConfiguration.EmailSender, email)
            {
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };

            _smtpClient.Send(mailMessage);
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
            using MailMessage mailMessage = new (_siteConfiguration.EmailSender, email)
            {
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };

            await _smtpClient.SendMailAsync(mailMessage);
        }
    }
}
