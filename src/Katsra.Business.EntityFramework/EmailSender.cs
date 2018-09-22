using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Kastra.Core.Business;
using Kastra.Core.Dto;
using Kastra.Core.Services;
using Microsoft.Extensions.Logging;

namespace Kastra.Business
{
    public class EmailSender : IEmailSender
    {
        private readonly SmtpClient _smtpClient;
        private readonly ILogger _logger;
        private readonly SiteConfigurationInfo _siteConfiguration;

        public EmailSender(IParameterManager parameterManager, ILogger<EmailSender> logger)
        {
            _logger = logger;

            // Get site configuration
            _siteConfiguration = parameterManager.GetSiteConfiguration();

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
            try
            {
                using (MailMessage mailMessage = new MailMessage(_siteConfiguration.EmailSender, email))
                {
                    mailMessage.Subject = subject;
                    mailMessage.Body = message;

                    _smtpClient.Send(mailMessage);
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
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
            try
            {
                using (MailMessage mailMessage = new MailMessage(_siteConfiguration.EmailSender, email))
                {
                    mailMessage.Subject = subject;
                    mailMessage.Body = message;

                    await _smtpClient.SendMailAsync(mailMessage);
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}
