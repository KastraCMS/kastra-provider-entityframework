using Kastra.Core.Dto;
using Kastra.DAL.EntityFramework.Models;

namespace Kastra.Business.Mappers
{
    public static class MailTemplateMapper
	{
        public static MailTemplateInfo ToMailTemplateInfo(this KastraMailTemplate mailTemplate)
		{
            if (mailTemplate is null)
            {
                return null;
            }

            MailTemplateInfo mailTemplateInfo = new MailTemplateInfo();
			mailTemplateInfo.MailtemplateId = mailTemplate.MailTemplateId;
			mailTemplateInfo.Keyname = mailTemplate.Keyname;
            mailTemplateInfo.Subject = mailTemplate.Subject;
            mailTemplateInfo.Message = mailTemplate.Message;

			return mailTemplateInfo;
		}

		public static KastraMailTemplate ToKastraMailTemplate(this MailTemplateInfo mailTemplateInfo)
		{
            if (mailTemplateInfo is null)
            {
                return null;
            }

            KastraMailTemplate mailTemplate = new KastraMailTemplate();
			mailTemplate.MailTemplateId = mailTemplateInfo.MailtemplateId;
            mailTemplate.Keyname = mailTemplateInfo.Keyname;
            mailTemplate.Subject = mailTemplateInfo.Subject;
            mailTemplate.Message = mailTemplateInfo.Message;

			return mailTemplate;
		}
	}
}
