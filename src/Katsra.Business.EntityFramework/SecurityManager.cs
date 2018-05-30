using System;
using System.Collections.Generic;
using System.Linq;
using Kastra.Business.Mappers;
using Kastra.Core;
using Kastra.Core.Business;
using Kastra.Core.Dto;
using Kastra.Core.Services;
using Kastra.DAL.EntityFramework;
using Kastra.DAL.EntityFramework.Models;

namespace Kastra.Business
{
    public class SecurityManager : ISecurityManager
    {
		#region Private members

		private readonly KastraContext _dbContext = null;
		private readonly CacheEngine _cacheEngine = null;

		#endregion

		public SecurityManager(KastraContext dbContext, CacheEngine cacheEngine)
        {
			_dbContext = dbContext;
			_cacheEngine = cacheEngine;
        }

        #region Module permission

        public IList<ModulePermissionInfo> GetModulePermissionsByModuleId(Int32 moduleId)
		{
			IList<ModulePermissionInfo> modulePermissions = _dbContext.KastraModulePermissions
																	  .Where(mp => mp.ModuleId == moduleId).ToList()
																	  .Select(mp => ModulePermissionMapper.ToModulePermissionInfo(mp)).ToList();
			return modulePermissions;
		}

		public Boolean SaveModulePermission(ModulePermissionInfo modulePermissionInfo)
		{
			KastraModulePermissions modulePermission = modulePermissionInfo.ToKastraModulePermission();

			if (modulePermissionInfo.ModulePermissionId > 0)
				_dbContext.KastraModulePermissions.Update(modulePermission);
			else
				_dbContext.KastraModulePermissions.Add(modulePermission);

			_dbContext.SaveChanges();

			// Clear cache
			_cacheEngine.ClearCacheContains("Module");

			return true;
		}

		public Boolean DeleteModulePermission(Int32 modulePermissionId)
		{
			if (modulePermissionId < 1)
				return false;

			KastraModulePermissions modulePermission = _dbContext.KastraModulePermissions.SingleOrDefault(p => p.ModulePermissionId == modulePermissionId);

			if (modulePermission == null)
				return false;

			_dbContext.KastraModulePermissions.Remove(modulePermission);
			_dbContext.SaveChanges();

			// Clear cache
			_cacheEngine.ClearCacheContains("Module");

			return true;
		}

		public Boolean DeleteModulePermission(Int32 moduleId, Int32 permissionId)
		{
			if (moduleId < 1)
				return false;

			KastraModulePermissions modulePermission = _dbContext.KastraModulePermissions.SingleOrDefault(p => p.ModuleId == moduleId && p.PermissionId == permissionId);

			if (modulePermission == null)
				return false;

			_dbContext.KastraModulePermissions.Remove(modulePermission);
			_dbContext.SaveChanges();

			// Clear cache
			_cacheEngine.ClearCacheContains("Module");

			return true;
		}

		#endregion

		#region Permissions

		public IList<PermissionInfo> GetPermissionsList()
		{
			IList<PermissionInfo> permissions = _dbContext.KastraPermissions.ToList()
														  .Select(p => PermissionMapper.ToPermissionInfo(p)).ToList();

			return permissions;
		}

        public bool SavePermission(PermissionInfo permissionInfo)
        {
			KastraPermissions permission = permissionInfo.ToKastraPermission();

			if (permissionInfo.PermissionId > 0)
				_dbContext.KastraPermissions.Update(permission);
			else
				_dbContext.KastraPermissions.Add(permission);

			_dbContext.SaveChanges();

            // Return permission id
            permissionInfo.PermissionId = permission.PermissionId;

			// Clear cache
			_cacheEngine.ClearCacheContains("Module");

			return true;
        }

        public Boolean DeletePermission(Int32 permissionId)
        {
			KastraPermissions permission = _dbContext.KastraPermissions.SingleOrDefault(p => p.PermissionId == permissionId);

			if (permission == null)
				return false;

			_dbContext.KastraPermissions.Remove(permission);
			_dbContext.SaveChanges();

			// Clear cache
			_cacheEngine.ClearCacheContains("Module");

			return true;
        }

        public PermissionInfo GetPermissionById(Int32 permissionId)
        {
            KastraPermissions permission = _dbContext.KastraPermissions.SingleOrDefault(p => p.PermissionId == permissionId);

            return permission?.ToPermissionInfo();
        }

        public PermissionInfo GetPermissionByName(String name)
        {
            KastraPermissions permission = _dbContext.KastraPermissions.SingleOrDefault(p => p.Name == name);

			return permission?.ToPermissionInfo();
        }

        #endregion
    }
}
