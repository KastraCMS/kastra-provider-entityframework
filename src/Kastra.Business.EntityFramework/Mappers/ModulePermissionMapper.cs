using Kastra.Core.DTO;
using Kastra.DAL.EntityFramework.Models;

namespace Kastra.Business.Mappers
{
    public static class ModulePermissionMapper
	{
		/// <summary>
		/// Convert ModulePermission to ModulePermissionInfo.
		/// </summary>
		/// <param name="modulePermission">Module permission</param>
		/// <returns>Module permission info</returns>
		public static ModulePermissionInfo ToModulePermissionInfo(this ModulePermission modulePermission)
		{
			if (modulePermission is null)
            {
				return null;
            }

			return new ModulePermissionInfo()
			{
				ModulePermissionId = modulePermission.ModulePermissionId,
				ModuleId = modulePermission.ModuleId,
				PermissionId = modulePermission.PermissionId,
			    Module = modulePermission.Module.ToModuleInfo(),
			    Permission = modulePermission.Permission.ToPermissionInfo(),
			};
		}

		/// <summary>
		/// Convert ModulePermissionInfo to ModulePermission.
		/// </summary>
		/// <param name="modulePermissionInfo">Module permission info</param>
		/// <returns>Module permission</returns>
		public static ModulePermission ToModulePermission(this ModulePermissionInfo modulePermissionInfo)
		{
			if (modulePermissionInfo is null)
            {
				return null;
            }

			return new ModulePermission()
			{
				ModulePermissionId = modulePermissionInfo.ModulePermissionId,
				ModuleId = modulePermissionInfo.ModuleId,
				PermissionId = modulePermissionInfo.PermissionId
			};
		}
	}
}
