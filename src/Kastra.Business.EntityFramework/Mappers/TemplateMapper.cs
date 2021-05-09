using System.Linq;
using Kastra.Core.DTO;
using Kastra.DAL.EntityFramework.Models;

namespace Kastra.Business.Mappers
{
    public static class TemplateMapper
	{
		/// <summary>
		/// Convert Template to TemplateInfo.
		/// </summary>
		/// <param name="template">Template</param>
		/// <param name="includePages">Convert the page list</param>
		/// <param name="includePlaces">Convert the place list</param>
		/// <returns>Template info</returns>
		public static TemplateInfo ToTemplateInfo(
			this PageTemplate template, 
			bool includePages = false, 
			bool includePlaces = false)
		{
			if (template is null)
            {
				return null;
            }

			var templateInfo = new TemplateInfo()
			{
				TemplateId = template.PageTemplateId,
				KeyName = template.KeyName,
				Name = template.Name,
				ModelClass = template.ModelClass,
				ViewPath = template.ViewPath
			};

            if(includePages)
            {
				templateInfo.Pages = template.KastraPages
					.Select(p => p.ToPageInfo())
					.ToList();
            }

			if (includePlaces)
            {
				templateInfo.Places = template.KastraPlaces
					.Select(p => p.ToPlaceInfo(false))
					.ToList();
            }

			return templateInfo;
		}

		/// <summary>
		/// Convert TemplateInfo to Template.
		/// </summary>
		/// <param name="templateInfo">Template info</param>
		/// <returns>Template</returns>
		public static PageTemplate ToPageTemplate(this TemplateInfo templateInfo)
		{
			if (templateInfo is null)
            {
				return null;
            }

			return new PageTemplate() 
			{
				PageTemplateId = templateInfo.TemplateId,
				KeyName = templateInfo.KeyName,
				Name = templateInfo.Name,
				ModelClass = templateInfo.ModelClass,
				ViewPath = templateInfo.ViewPath
			};
		}
	}
}
