using System.Linq;
using Kastra.Core.DTO;
using Kastra.DAL.EntityFramework.Models;

namespace Kastra.Business.Mappers
{
    public static class ModuleMapper
	{
		/// <summary>
		/// Convert Module to ModuleInfo.
		/// </summary>
		/// <param name="module">Module</param>
		/// <param name="includePermissions">Convert permission list</param>
		/// <returns>Module info</returns>
		public static ModuleInfo ToModuleInfo(this Module module, bool includePermissions = false)
		{
			var moduleInfo = new ModuleInfo()
			{
				ModuleId = module.ModuleId,
				Name = module.Name,
				PageId = module.PageId,
				PlaceId = module.PlaceId,
				ModuleDefId = module.ModuleDefinitionId,
				IsDisabled = module.IsDisabled
			};

            if(module.ModuleDefinition is not null)
            {
			    moduleInfo.ModuleDefinition = module.ModuleDefinition.ToModuleDefinitionInfo(false, true);
            }

            if (module.Place is not null)
            {
			    moduleInfo.Place = module.Place.ToPlaceInfo();
            }

            if(includePermissions && module.ModulePermissions is not null)
            {
			    moduleInfo.ModulePermissions = module.ModulePermissions
					.Select(mp => mp.ToModulePermissionInfo())
					.ToList();
            }

			return moduleInfo;
		}

		/// <summary>
		/// Convert ModuleInfo to Module.
		/// </summary>
		/// <param name="moduleInfo">Module info</param>
		/// <returns>Module</returns>
		public static Module ToModule(this ModuleInfo moduleInfo)
		{
			if (moduleInfo is null)
            {
				return null;
            }

			return new Module()
			{
				ModuleId = moduleInfo.ModuleId,
				ModuleDefinitionId = moduleInfo.ModuleDefId,
				PageId = moduleInfo.PageId,
				PlaceId = moduleInfo.PlaceId,
				Name = moduleInfo.Name,
				IsDisabled = moduleInfo.IsDisabled
			};
		}
	}
}
