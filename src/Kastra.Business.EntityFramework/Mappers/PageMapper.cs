using System;
using Kastra.Core;
using Kastra.Core.Dto;
using Kastra.DAL.EntityFramework.Models;

namespace Kastra.Business.Mappers
{
	public static class PageMapper
	{
		public static PageInfo ToPageInfo(this KastraPages page, Boolean includeChildren = false)
		{
			if (page == null)
				return null;

			PageInfo pageInfo = new PageInfo();
			pageInfo.PageId = page.PageId;
			pageInfo.Title = page.Title;
			pageInfo.KeyName = page.KeyName;
			pageInfo.PageTemplateId = page.PageTemplateId;
            pageInfo.MetaKeywords = page.MetaKeywords;
            pageInfo.MetaDescription = page.MetaDescription;
            pageInfo.MetaRobot = page.MetaRobot;

            if(page.PageTemplate != null)
                pageInfo.PageTemplate = TemplateMapper.ToTemplateInfo(page.PageTemplate, includeChildren);

			return pageInfo;
		}

		public static KastraPages ToKastraPage(this PageInfo pageInfo)
		{
			if (pageInfo == null)
				return null;

			KastraPages page = new KastraPages();
			page.PageId = pageInfo.PageId;
			page.Title = pageInfo.Title;
			page.KeyName = pageInfo.KeyName;
			page.PageTemplateId = pageInfo.PageTemplateId;
			page.MetaKeywords = pageInfo.MetaKeywords;
			page.MetaDescription = pageInfo.MetaDescription;
			page.MetaRobot = pageInfo.MetaRobot;

			return page;
		}
	}
}
