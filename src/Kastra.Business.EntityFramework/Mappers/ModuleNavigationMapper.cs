using Kastra.Core.DTO;
using Kastra.DAL.EntityFramework.Models;

namespace Kastra.Business.Mappers
{
    public static class ModuleNavigationMapper
    {
		/// <summary>
		/// Convert ModuleNavigation to ModuleNavigationInfo.
		/// </summary>
		/// <param name="moduleNavigation">Module navigation</param>
		/// <returns>Module navigation info</returns>
		public static ModuleNavigationInfo ToModuleNavigationInfo(this ModuleNavigation moduleNavigation)
		{
			if (moduleNavigation is null)
			{
				return null;
			}

			return new ModuleNavigationInfo()
			{
				Id = moduleNavigation.Id,
				Name = moduleNavigation.Name,
				Url = moduleNavigation.Url,
				Type = moduleNavigation.Type,
				Icon = moduleNavigation.Icon,
				ModuleDefinitionId = moduleNavigation.ModuleDefinitionId,
				ModuleDefinition = moduleNavigation.ModuleDefinition.ToModuleDefinitionInfo()
			};
		}

		/// <summary>
		/// Convert ModuleNavigationInfo to ModuleNavigation.
		/// </summary>
		/// <param name="moduleNavigationInfo">Module navigation info</param>
		/// <returns>Module navigation</returns>
		public static ModuleNavigation ToModuleNavigation(this ModuleNavigationInfo moduleNavigationInfo)
		{
			if (moduleNavigationInfo is null)
			{
				return null;
			}

			return new ModuleNavigation()
			{
				Id = moduleNavigationInfo.Id,
				ModuleDefinitionId = moduleNavigationInfo.ModuleDefinitionId,
				Name = moduleNavigationInfo.Name,
				Type = moduleNavigationInfo.Type,
				Icon = moduleNavigationInfo.Icon,
				Url = moduleNavigationInfo.Url
			};
		}
	}
}
