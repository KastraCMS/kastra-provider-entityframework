using System;
using System.Linq;
using Kastra.Core;
using Kastra.Core.Dto;
using Kastra.DAL.EntityFramework.Models;

namespace Kastra.Business.Mappers
{
	public static class PlaceMapper
	{
        public static PlaceInfo ToPlaceInfo(this KastraPlaces place, Boolean includeChildren = false)
		{
            PlaceInfo placeInfo = new PlaceInfo();
			placeInfo.PlaceId = place.PlaceId;
			placeInfo.KeyName = place.KeyName;
			placeInfo.PageTemplateId = place.PageTemplateId;

            if(includeChildren)
			    placeInfo.Modules = place.KastraModules.Select(m => ModuleMapper.ToModuleInfo(m, true))?.ToList();

            if(place.PageTemplate != null)
			    placeInfo.Template = TemplateMapper.ToTemplateInfo(place.PageTemplate);

			return placeInfo;
		}

		public static KastraPlaces ToKastraPlace(this PlaceInfo placeInfo)
		{
			KastraPlaces place = new KastraPlaces();
			place.PlaceId = placeInfo.PlaceId;
			place.KeyName = placeInfo.KeyName;
			place.PageTemplateId = placeInfo.PageTemplateId;

			return place;
		}
	}
}
