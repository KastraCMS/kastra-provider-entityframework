using System;
using System.Collections.Generic;
using System.Linq;
using Kastra.Business.Mappers;
using Kastra.Core.Services.Contracts;
using Kastra.Core.DTO;
using Kastra.Core.Services;
using Kastra.DAL.EntityFramework;
using Kastra.DAL.EntityFramework.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Kastra.Business
{
    public class SecurityManager : ISecurityManager
    {
		#region Private members

		private readonly KastraDbContext _dbContext;
		private readonly CacheEngine _cacheEngine;

		#endregion

		public SecurityManager(KastraDbContext dbContext, CacheEngine cacheEngine)
        {
			_dbContext = dbContext;
			_cacheEngine = cacheEngine;
        }

        #region Module permission

		/// <inheritdoc cref="ISecurityManager.GetModulePermissionsByModuleIdAsync(int)" />
        public async Task<IList<ModulePermissionInfo>> GetModulePermissionsByModuleIdAsync(int moduleId)
		{
			IList<ModulePermissionInfo> modulePermissions = await _dbContext.KastraModulePermissions
																	  .Where(mp => mp.ModuleId == moduleId)
																	  .Select(mp => mp.ToModulePermissionInfo())
																	  .ToListAsync();
			return modulePermissions;
		}

		/// <inheritdoc cref="ISecurityManager.SaveModulePermissionAsync(ModulePermissionInfo)" />
		public async Task<bool> SaveModulePermissionAsync(ModulePermissionInfo modulePermissionInfo)
		{
			ModulePermission modulePermission = modulePermissionInfo.ToModulePermission();

			if (modulePermissionInfo.ModulePermissionId > 0)
            {
				_dbContext.KastraModulePermissions.Update(modulePermission);
            }
			else
            {
				_dbContext.KastraModulePermissions.Add(modulePermission);
            }

			await _dbContext.SaveChangesAsync();

			// Clear cache
			_cacheEngine.ClearCacheContains("Module");

			return true;
		}

		/// <inheritdoc cref="ISecurityManager.DeleteModulePermissionAsync(int)" />
		public async Task<bool> DeleteModulePermissionAsync(int modulePermissionId)
		{
			if (modulePermissionId < 1)
            {
				return false;
            }

			ModulePermission modulePermission = await _dbContext.KastraModulePermissions.SingleOrDefaultAsync(p => p.ModulePermissionId == modulePermissionId);

			if (modulePermission is null)
            {
				return false;
            }

			_dbContext.KastraModulePermissions.Remove(modulePermission);

			await _dbContext.SaveChangesAsync();

			// Clear cache
			_cacheEngine.ClearCacheContains("Module");

			return true;
		}

		/// <inheritdoc cref="ISecurityManager.DeleteModulePermissionAsync(int)"/>
		public async Task<bool> DeleteModulePermissionAsync(int moduleId, int permissionId)
		{
			if (moduleId < 1)
            {
				return false;
            }

			ModulePermission modulePermission = await _dbContext.KastraModulePermissions.SingleOrDefaultAsync(p => p.ModuleId == moduleId && p.PermissionId == permissionId);

			if (modulePermission is null)
            {
				return false;
            }

			_dbContext.KastraModulePermissions.Remove(modulePermission);

			await _dbContext.SaveChangesAsync();

			// Clear cache
			_cacheEngine.ClearCacheContains("Module");

			return true;
		}

		#endregion

		#region Permissions

		/// <inheritdoc cref="ISecurityManager.GetPermissionsListAsync" />
		public async Task<IList<PermissionInfo>> GetPermissionsListAsync()
		{
			IList<PermissionInfo> permissions = await _dbContext.KastraPermissions
													.Select(p => p.ToPermissionInfo(false))
													.ToListAsync();

			return permissions;
		}

		/// <inheritdoc cref="ISecurityManager.SavePermissionAsync(PermissionInfo)" />
        public async Task<bool> SavePermissionAsync(PermissionInfo permissionInfo)
        {
			Permission permission = permissionInfo.ToPermission();

			if (permissionInfo.PermissionId > 0)
            {
				_dbContext.KastraPermissions.Update(permission);
            }
			else
            {
				_dbContext.KastraPermissions.Add(permission);
            }

			await _dbContext.SaveChangesAsync();

            // Return permission id
            permissionInfo.PermissionId = permission.PermissionId;

			// Clear cache
			_cacheEngine.ClearCacheContains("Module");

			return true;
        }

		/// <inheritdoc cref="ISecurityManager.DeletePermissionAsync(int)" />
        public async Task<bool> DeletePermissionAsync(int permissionId)
        {
			Permission permission = await _dbContext.KastraPermissions.SingleOrDefaultAsync(p => p.PermissionId == permissionId);

			if (permission is null)
            {
				return false;
            }

			_dbContext.KastraPermissions.Remove(permission);

			await _dbContext.SaveChangesAsync();

			// Clear cache
			_cacheEngine.ClearCacheContains("Module");

			return true;
        }

		/// <inheritdoc cref="ISecurityManager.GetPermissionByIdAsync(int)" />
        public async Task<PermissionInfo> GetPermissionByIdAsync(int permissionId)
        {
            Permission permission = await _dbContext.KastraPermissions.SingleOrDefaultAsync(p => p.PermissionId == permissionId);

            return permission.ToPermissionInfo();
        }

		/// <inheritdoc cref="ISecurityManager.GetPermissionByNameAsync(string)" />
        public async Task<PermissionInfo> GetPermissionByNameAsync(string name)
        {
            Permission permission = await _dbContext.KastraPermissions.SingleOrDefaultAsync(p => p.Name == name);

			return permission.ToPermissionInfo();
        }

        #endregion
    }
}
