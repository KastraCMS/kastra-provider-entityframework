using System.Linq;
using Kastra.Core.DTO;
using Kastra.DAL.EntityFramework.Models;

namespace Kastra.Business.Mappers
{
    public static class PlaceMapper
	{
		/// <summary>
		/// Convert Place to PlaceInfo.
		/// </summary>
		/// <param name="place">Place</param>
		/// <param name="includeModules">Convert the module list</param>
		/// <returns></returns>
        public static PlaceInfo ToPlaceInfo(this Place place, bool includeModules = false)
		{
			if (place is null)
            {
				return null;
            }

			var placeInfo = new PlaceInfo()
			{
				PlaceId = place.PlaceId,
				KeyName = place.KeyName,
				PageTemplateId = place.PageTemplateId,
				ModuleId = place.ModuleId,
				Template = place.PageTemplate.ToTemplateInfo()
			};

			if (includeModules)
            {
			    placeInfo.Modules = place.KastraModules
					.Select(m => m.ToModuleInfo(true))
					.ToList();
            }

			return placeInfo;
		}

		/// <summary>
		/// Convert PlaceInfo to Place.
		/// </summary>
		/// <param name="placeInfo">Place info</param>
		/// <returns>Place</returns>
		public static Place ToPlace(this PlaceInfo placeInfo)
		{
			if (placeInfo is null)
            {
				return null;
            }

			return new Place()
			{
				PlaceId = placeInfo.PlaceId,
				KeyName = placeInfo.KeyName,
				PageTemplateId = placeInfo.PageTemplateId
			};
		}
	}
}
