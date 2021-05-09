using System.Linq;
using Kastra.Core.DTO;
using Kastra.DAL.EntityFramework.Models;

namespace Kastra.Business.Mappers
{
    public static class PermissionMapper
	{
		/// <summary>
		/// Convert Permission to PermissionInfo.
		/// </summary>
		/// <param name="permission">Permission</param>
		/// <param name="includeModulePermissions">Convert the module permission list</param>
		/// <returns>Permission info</returns>
		public static PermissionInfo ToPermissionInfo(this Permission permission, bool includeModulePermissions = false)
		{
			if (permission is null)
            {
				return null;
            }

			var permissionInfo = new PermissionInfo()
			{
				PermissionId = permission.PermissionId,
				Name = permission.Name
			};

            if(includeModulePermissions)
            {
			    permissionInfo.ModulePermissions = permission.KastraModulePermissions.Select(mp => mp.ToModulePermissionInfo()).ToList();
            }

			return permissionInfo;
		}

		/// <summary>
		/// Convert PermissionInfo to Permission.
		/// </summary>
		/// <param name="permissionInfo">Permission info</param>
		/// <returns>Permission</returns>
        public static Permission ToPermission(this PermissionInfo permissionInfo)
        {
            if (permissionInfo is null)
            {
                return null;
            }

            return new Permission()
			{
				PermissionId = permissionInfo.PermissionId,
				Name = permissionInfo.Name
			};
        }
	}
}
