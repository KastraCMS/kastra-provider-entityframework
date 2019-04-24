using System;
using System.Linq;
using Kastra.Core;
using Kastra.Core.Dto;
using Kastra.DAL.EntityFramework.Models;

namespace Kastra.Business.Mappers
{
	public static class TemplateMapper
	{
		public static TemplateInfo ToTemplateInfo(this KastraPageTemplates template, Boolean includeChildren = false)
		{
			TemplateInfo templateInfo = new TemplateInfo();
			templateInfo.TemplateId = template.PageTemplateId;
			templateInfo.KeyName = template.KeyName;
			templateInfo.Name = template.Name;
			templateInfo.ModelClass = template.ModelClass;
			templateInfo.ViewPath = template.ViewPath;

            if(includeChildren)
            {
				templateInfo.Pages = template.KastraPages.Select(p => PageMapper.ToPageInfo(p, false)).ToList();
				templateInfo.Places = template.KastraPlaces.Select(p => PlaceMapper.ToPlaceInfo(p, false)).ToList();   
            }

			return templateInfo;
		}

		public static KastraPageTemplates ToKastraPageTemplate(this TemplateInfo templateInfo)
		{
			KastraPageTemplates template = new KastraPageTemplates();
			template.PageTemplateId = templateInfo.TemplateId;
			template.KeyName = templateInfo.KeyName;
			template.Name = templateInfo.Name;
			template.ModelClass = templateInfo.ModelClass;
			template.ViewPath = templateInfo.ViewPath;

			return template;
		}
	}
}
