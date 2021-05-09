using Kastra.Core.DTO;
using Kastra.DAL.EntityFramework.Models;

namespace Kastra.Business.Mappers
{
    public static class MailTemplateMapper
	{
        /// <summary>
        /// Convert MailtTemplate to MailTemplateInfo.
        /// </summary>
        /// <param name="mailTemplate">Mail template</param>
        /// <returns>Mail template info</returns>
        public static MailTemplateInfo ToMailTemplateInfo(this MailTemplate mailTemplate)
		{
            if (mailTemplate is null)
            {
                return null;
            }

			return new MailTemplateInfo()
            {
                MailtemplateId = mailTemplate.MailTemplateId,
                Keyname = mailTemplate.Keyname,
                Subject = mailTemplate.Subject,
                Message = mailTemplate.Message
            };
		}

        /// <summary>
        /// Convert MailTemplateInfo to MailTemplate.
        /// </summary>
        /// <param name="mailTemplateInfo">Mail template info</param>
        /// <returns>Mail template</returns>
		public static MailTemplate ToMailTemplate(this MailTemplateInfo mailTemplateInfo)
		{
            if (mailTemplateInfo is null)
            {
                return null;
            }

			return new MailTemplate()
            {
                MailTemplateId = mailTemplateInfo.MailtemplateId,
                Keyname = mailTemplateInfo.Keyname,
                Subject = mailTemplateInfo.Subject,
                Message = mailTemplateInfo.Message
            };
		}
	}
}
