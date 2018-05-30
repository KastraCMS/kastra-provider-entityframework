using System;
using Kastra.Core;
using Kastra.Core.Dto;
using Kastra.DAL.EntityFramework.Models;

namespace Kastra.Business.Mappers
{
	public static class ModulePermissionMapper
	{
		public static ModulePermissionInfo ToModulePermissionInfo(this KastraModulePermissions modulePermission, Boolean includeChildren = false)
		{
			ModulePermissionInfo modulePermissionInfo = new ModulePermissionInfo();
			modulePermissionInfo.ModulePermissionId = modulePermission.ModulePermissionId;
			modulePermissionInfo.ModuleId = modulePermission.ModuleId;
			modulePermissionInfo.PermissionId = modulePermission.PermissionId;

            if(modulePermission.Module != null)
			    modulePermissionInfo.Module = ModuleMapper.ToModuleInfo(modulePermission.Module);

            if(modulePermission.Permission != null)
			    modulePermissionInfo.Permission = PermissionMapper.ToPermissionInfo(modulePermission.Permission);

			return modulePermissionInfo;
		}

		public static KastraModulePermissions ToKastraModulePermission(this ModulePermissionInfo modulePermissionInfo)
		{
			KastraModulePermissions modulePermission = new KastraModulePermissions();
			modulePermission.ModulePermissionId = modulePermissionInfo.ModulePermissionId;
			modulePermission.ModuleId = modulePermissionInfo.ModuleId;
			modulePermission.PermissionId = modulePermissionInfo.PermissionId;

			return modulePermission;
		}
	}
}
