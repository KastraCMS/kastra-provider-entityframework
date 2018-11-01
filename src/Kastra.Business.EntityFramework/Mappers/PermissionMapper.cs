using System;
using System.Linq;
using Kastra.Core;
using Kastra.Core.Dto;
using Kastra.DAL.EntityFramework.Models;

namespace Kastra.Business.Mappers
{
	public static class PermissionMapper
	{
		public static PermissionInfo ToPermissionInfo(this KastraPermissions permission, Boolean includeChildren = false)
		{
			PermissionInfo permissionInfo = new PermissionInfo();
			permissionInfo.PermissionId = permission.PermissionId;
			permissionInfo.Name = permission.Name;

            if(includeChildren)
			    permissionInfo.ModulePermissions = permission.KastraModulePermissions.Select(mp => ModulePermissionMapper.ToModulePermissionInfo(mp, false)).ToList();

			return permissionInfo;
		}

        public static KastraPermissions ToKastraPermission(this PermissionInfo permissionInfo)
        {
            if (permissionInfo == null)
                return null;

            KastraPermissions permission = new KastraPermissions();
            permission.PermissionId = permissionInfo.PermissionId;
            permission.Name = permissionInfo.Name;

            return permission;
        }
	}
}
