using Kastra.Core.DTO;
using Kastra.DAL.EntityFramework.Models;

namespace Kastra.Business.Mappers
{
    public static class PageMapper
	{
		/// <summary>
		/// Convert Page to PageInfo.
		/// </summary>
		/// <param name="page">Page</param>
		/// <param name="includeTemplatePages">Convert template page list</param>
		/// <param name="includeTemplatePlaces">Convert template place list</param>
		/// <returns>Page info</returns>
		public static PageInfo ToPageInfo(
			this Page page, 
			bool includeTemplatePages = false, 
			bool includeTemplatePlaces = false)
		{
			if (page is null)
            {
				return null;
            }

			return new PageInfo()
			{
				PageId = page.PageId,
				Title = page.Title,
				KeyName = page.KeyName,
				PageTemplateId = page.PageTemplateId,
				MetaKeywords = page.MetaKeywords,
				MetaDescription = page.MetaDescription,
				MetaRobot = page.MetaRobot,
				PageTemplate = page.PageTemplate.ToTemplateInfo(includeTemplatePages, includeTemplatePlaces)
			};
		}

		/// <summary>
		/// Convert PageInfo to Page.
		/// </summary>
		/// <param name="pageInfo">Page info</param>
		/// <returns>Page</returns>
		public static Page ToPage(this PageInfo pageInfo)
		{
			if (pageInfo is null)
            {
				return null;
            }

			return new Page()
			{
				PageId = pageInfo.PageId,
				Title = pageInfo.Title,
				KeyName = pageInfo.KeyName,
				PageTemplateId = pageInfo.PageTemplateId,
				MetaKeywords = pageInfo.MetaKeywords,
				MetaDescription = pageInfo.MetaDescription,
				MetaRobot = pageInfo.MetaRobot
			};
		}
	}
}
