using System.Collections.Generic;
using System.Linq;
using Kastra.Business.Mappers;
using Kastra.Core.Services.Contracts;
using Kastra.Core.Constants;
using Kastra.Core.DTO;
using Kastra.Core.Services;
using Kastra.DAL.EntityFramework;
using Kastra.DAL.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Kastra.Business
{
    public class ViewManager : IViewManager
    {
        #region Private members

        private readonly KastraDbContext _dbContext;
        private readonly CacheEngine _cacheEngine;

        #endregion

        public ViewManager(KastraDbContext dbContext, CacheEngine cacheEngine)
        {
            _dbContext = dbContext;
            _cacheEngine = cacheEngine;
        }

        #region Pages

        /// <inheritdoc cref="IViewManager.GetPagesListAsync" />
        public async Task<IList<PageInfo>> GetPagesListAsync()
        {
            if (_dbContext is null)
            {
                return null;
            }

            return await _dbContext.KastraPages
                .Select(p => p.ToPageInfo(false, false))
                .ToListAsync();
        }

        /// <inheritdoc cref="IViewManager.SavePageAsync(PageInfo)" />
        public async Task<bool> SavePageAsync(PageInfo page)
        {
            if (page is null)
            {
                return false;
            }

            Page currentPage = await _dbContext.KastraPages.SingleOrDefaultAsync(p => p.PageId == page.PageId);

            if(currentPage is not null)
            {
                if (currentPage.PageTemplateId != page.PageTemplateId)
                {
                    // Get the modules of the page
                    List<Module> modules = await _dbContext.KastraModules
                                                .Where(m => m.PageId == currentPage.PageId)
                                                .ToListAsync();

                    // Remove 
                    foreach (Module module in modules)
                    {
                        module.IsDisabled = true;
                    }
                    
                    _cacheEngine.ClearCacheContains("Module");
                }

                currentPage.KeyName = page.KeyName;
                currentPage.MetaDescription = page.MetaDescription;
                currentPage.MetaKeywords = page.MetaKeywords;
                currentPage.MetaRobot = page.MetaRobot;
                currentPage.PageTemplateId = page.PageTemplateId;
                currentPage.Title = page.Title;

                 _dbContext.KastraPages.Update(currentPage);
            }
            else
            {
                _dbContext.KastraPages.Add(page.ToPage());
            }

            await _dbContext.SaveChangesAsync();

            // Clear cache
            _cacheEngine.ClearCacheContains("Page");

            return true;
        }

        /// <inheritdoc cref="IViewManager.GetPageAsync(int, bool)" />
        public async Task<PageInfo> GetPageAsync(int pageID, bool getAll = false)
        {
            Page page = null;
            string pageCacheKey = string.Format(PageConfiguration.PageCacheKey, pageID);

            if (_cacheEngine.GetCacheObject(pageCacheKey, out PageInfo pageInfo))
            {
                return pageInfo;
            }

            if (getAll)
            {
                page = await _dbContext.KastraPages.Include(p => p.PageTemplate.KastraPlaces)
                              .SingleOrDefaultAsync(p => p.PageId == pageID);

                if (page is null)
                {
                    return null;
                }

                pageInfo = page.ToPageInfo(true, true);

                foreach (var place in pageInfo.PageTemplate.Places)
                {
                    if(place.ModuleId.HasValue)
                    {
                        place.StaticModule = await GetModuleAsync(place.ModuleId.Value, true, false);
                    }
                    
                    place.Modules = await GetModulesListByPlaceIdAsync(place.PlaceId, true);
                }
            }
            else
            {
                page = await _dbContext.KastraPages.SingleOrDefaultAsync(p => p.PageId == pageID);

                if (page is null)
                {
                    return null;
                }

                pageInfo = page.ToPageInfo();
            }

            _cacheEngine.SetCacheObject(pageCacheKey, pageInfo);

            return pageInfo;
        }

        /// <inheritdoc cref="IViewManager.GetPageByKeyAsync(string, bool)" />
        public async Task<PageInfo> GetPageByKeyAsync(string keyName, bool getAll = false)
        {
            Page page = null;
            string pageCacheKey = string.Format(PageConfiguration.PageByKeyCacheKey, keyName);

            if (_cacheEngine.GetCacheObject(pageCacheKey, out PageInfo pageInfo))
            {
                return pageInfo;
            }

            if (getAll)
            {
                page = await _dbContext.KastraPages
                    .Include(p => p.PageTemplate.KastraPlaces)
                    .SingleOrDefaultAsync(p => p.KeyName == keyName);

                if (page is null)
                {
                    return null;
                }

                pageInfo = page.ToPageInfo(true, true);

                foreach (var place in pageInfo.PageTemplate.Places)
                {
                    if(place.ModuleId.HasValue)
                    {
                        place.StaticModule = await GetModuleAsync(place.ModuleId.Value, true, false);
                    }

                    place.Modules = await GetModulesListByPlaceIdAsync(place.PlaceId, true);
                }
                    
            }
            else
            {
                page = await _dbContext.KastraPages.SingleOrDefaultAsync(p => p.KeyName == keyName);

                if (page is null)
                {
                    return null;
                }

                pageInfo = page.ToPageInfo();
            }

            _cacheEngine.SetCacheObject(pageCacheKey, pageInfo);

            return pageInfo;
        }

        /// <inheritdoc cref="IViewManager.DeletePageAsync(int)" />
        public async Task<bool> DeletePageAsync(int pageID)
        {
            if (pageID < 1)
            {
                return false;
            }

            Page page = await _dbContext.KastraPages.SingleOrDefaultAsync(p => p.PageId == pageID);

            if (page is null)
            {
                return false;
            }

            _dbContext.KastraPages.Remove(page);

            await _dbContext.SaveChangesAsync();

            // Clear cache
            _cacheEngine.ClearCacheContains("Page");

            return true;
        }

        #endregion

        #region Page template

        /// <inheritdoc cref="IViewManager.GetPageTemplatesListAsync" />
        public async Task<IList<TemplateInfo>> GetPageTemplatesListAsync()
        {
            if (_dbContext is null)
            {
                return null;
            }

            return await _dbContext.KastraPageTemplates
                .Select(t => t.ToTemplateInfo(false, false))
                .ToListAsync();
        }

        /// <inheritdoc cref="IViewManager.GetPageTemplateAsync(int)" />
        public async Task<TemplateInfo> GetPageTemplateAsync(int pageTemplateID)
        {
            return (await _dbContext.KastraPageTemplates
                .SingleOrDefaultAsync(pt => pt.PageTemplateId == pageTemplateID))
                .ToTemplateInfo();
        }

        /// <inheritdoc cref="IViewManager.SavePageTemplateAsync(TemplateInfo)" />
        public async Task<bool> SavePageTemplateAsync(TemplateInfo templateInfo)
        {
            PageTemplate template = null;

            if (templateInfo is null)
            {
                return false;
            }

            if (templateInfo.TemplateId > 0)
            {
                template = await _dbContext.KastraPageTemplates
                    .SingleOrDefaultAsync(pt => pt.PageTemplateId == templateInfo.TemplateId);
            }

            if (template is null)
            {
                template = new PageTemplate();
            }

            template.Name = templateInfo.Name;
            template.KeyName = templateInfo.KeyName;
            template.ModelClass = templateInfo.ModelClass;
            template.ViewPath = templateInfo.ViewPath;

            if (templateInfo.TemplateId > 0)
            {
                _dbContext.KastraPageTemplates.Update(template);
            }
            else
            {
                _dbContext.KastraPageTemplates.Add(template);
            }

            await _dbContext.SaveChangesAsync();

            templateInfo.TemplateId = template.PageTemplateId;

            // Clear cache
            _cacheEngine.ClearCacheContains("Template");

            return true;
        }

        /// <inheritdoc cref="IViewManager.DeletePageTemplateAsync(int)" />
        public async Task<bool> DeletePageTemplateAsync(int pageTemplateID)
        {
            if (pageTemplateID < 1)
            {
                return false;
            }

            PageTemplate pageTemplate = await _dbContext.KastraPageTemplates
                .SingleOrDefaultAsync(p => p.PageTemplateId == pageTemplateID);

            if (pageTemplate is null)
            {
                return false;
            }

            _dbContext.KastraPageTemplates.Remove(pageTemplate);

            await _dbContext.SaveChangesAsync();

            // Clear cache
            _cacheEngine.ClearCacheContains("Template");

            return true;
        }

        #endregion

        #region Place

        /// <inheritdoc cref="IViewManager.GetPlacesListAsync(bool)" />
        public async Task<IList<PlaceInfo>> GetPlacesListAsync(bool includeModules = false)
        {
            return await _dbContext.KastraPlaces
                .Include(p => p.KastraModules)
                .Select(p => p.ToPlaceInfo(includeModules))
                .ToListAsync();
        }

        /// <inheritdoc cref="IViewManager.GetPlaceAsync(int)" />
        public async Task<PlaceInfo> GetPlaceAsync(int placeID)
        {
            return (await _dbContext.KastraPlaces
                .SingleOrDefaultAsync(pt => pt.PlaceId == placeID))
                .ToPlaceInfo();
        }

        /// <inheritdoc cref="IViewManager.SavePlaceAsync(PlaceInfo)" />
        public async Task<bool> SavePlaceAsync(PlaceInfo place)
        {
            if (place is null)
            {
                return false;
            }

            Place currentPlace = await _dbContext.KastraPlaces
                .SingleOrDefaultAsync(p => p.PlaceId == place.PlaceId);

            if (currentPlace != null)
            {
                currentPlace.KeyName = place.KeyName;
                currentPlace.PageTemplateId = place.PageTemplateId;
                currentPlace.PlaceId = place.PlaceId;
                currentPlace.ModuleId = place.ModuleId;

                _dbContext.KastraPlaces.Update(currentPlace);
            }
            else
            {
                _dbContext.KastraPlaces.Add(place.ToPlace());
            }

            await _dbContext.SaveChangesAsync();

            // Clear cache
            _cacheEngine.ClearCacheContains("Place");

            return true;
        }

        /// <inheritdoc cref="IViewManager.DeletePlaceAsync(int)" />
        public async Task<bool> DeletePlaceAsync(int placeID)
        {
            if (placeID < 1)
            {
                return false;
            }

            Place place = await _dbContext.KastraPlaces
                .SingleOrDefaultAsync(p => p.PlaceId == placeID);

            if (place is null)
            {
                return false;
            }

            _dbContext.KastraPlaces.Remove(place);

            await _dbContext.SaveChangesAsync();

            // Clear cache
            _cacheEngine.ClearCacheContains("Place");

            return true;
        }

        #endregion

        #region Module Definition

        /// <inheritdoc cref="IViewManager.GetModuleDefAsync(int, bool)" />
        public async Task<ModuleDefinitionInfo> GetModuleDefAsync(int moduleDefID, bool getModuleControls = false)
        {
            ModuleDefinition moduleDefinition = null;

            if (getModuleControls)
            {
                moduleDefinition = await _dbContext.KastraModuleDefinitions
                    .Include(md => md.ModuleControls)
                    .SingleOrDefaultAsync(pt => pt.ModuleDefId == moduleDefID);
            }
            else
            {
                moduleDefinition = await _dbContext.KastraModuleDefinitions
                    .SingleOrDefaultAsync(pt => pt.ModuleDefId == moduleDefID);
            }

            return moduleDefinition.ToModuleDefinitionInfo(true, true);
        }

        /// <inheritdoc cref="IViewManager.GetModuleDefsListAsync" />
        public async Task<IList<ModuleDefinitionInfo>> GetModuleDefsListAsync()
        {
            return await _dbContext.KastraModuleDefinitions
                .Select(md => md.ToModuleDefinitionInfo(false, false))
                .ToListAsync();
        }

        /// <inheritdoc cref="IViewManager.SaveModuleDefAsync(ModuleDefinitionInfo)" />
        public async Task<bool> SaveModuleDefAsync(ModuleDefinitionInfo moduleDefinition)
        {
            ModuleDefinition newModuleDefinition;

            if (moduleDefinition is null)
            {
                return false;
            }

            newModuleDefinition = moduleDefinition.ToModuleDefinition();

            if (newModuleDefinition.ModuleDefId > 0)
            {
                _dbContext.KastraModuleDefinitions.Update(newModuleDefinition);
            }
            else
            {
                _dbContext.KastraModuleDefinitions.Add(newModuleDefinition);
            }

            await _dbContext.SaveChangesAsync();

            // Clear cache
            _cacheEngine.ClearCacheContains("Module");

            return true;
        }

        /// <inheritdoc cref="IViewManager.DeleteModuleDefAsync(int)" />
        public async Task<bool> DeleteModuleDefAsync(int moduleDefinitionId)
        {
            if (moduleDefinitionId < 1)
            {
                return false;
            }

            ModuleDefinition moduleDef = await _dbContext.KastraModuleDefinitions
                .SingleOrDefaultAsync(p => p.ModuleDefId == moduleDefinitionId);

            if (moduleDef is null)
            {
                return false;
            }

            _dbContext.KastraModuleDefinitions.Remove(moduleDef);

            await _dbContext.SaveChangesAsync();

            // Clear cache
            _cacheEngine.ClearCacheContains("Module");

            return true;
        }

        #endregion

        #region Module

        /// <inheritdoc cref="IViewManager.GetModuleAsync(int, bool, bool)" />
        public async Task<ModuleInfo> GetModuleAsync(
            int moduleID, 
            bool includeModuleDef = false, 
            bool includePlace = false
        ) {
            IQueryable<Module> query = _dbContext.KastraModules
                .Include(m => m.ModulePermissions)
                .ThenInclude(m => m.Permission);

            if (includeModuleDef)
            {
                query = query.Include(m => m.ModuleDefinition.ModuleControls);
            }

            if (includePlace)
            {
                query = query.Include(m => m.Place);
            }

            return (await query
                .SingleOrDefaultAsync(pt => pt.ModuleId == moduleID))
                .ToModuleInfo(true);
        }
        
        /// <inheritdoc cref="IViewManager.GetModulesListAsync(bool)" />
        public async Task<IList<ModuleInfo>> GetModulesListAsync(bool includeModuleControls = false)
        {
            if (includeModuleControls)
            {
                return await _dbContext.KastraModules
                    .Include(m => m.ModuleDefinition.ModuleControls)
                    .Select(m => m.ToModuleInfo(false))
                    .ToListAsync();
            }
            else
            {
                return await _dbContext.KastraModules
                    .Select(m => m.ToModuleInfo(false))
                    .ToListAsync();
            }
        }

        /// <inheritdoc cref="IViewManager.GetModulesListByPlaceIdAsync(int, bool)"/>
        public async Task<IList<ModuleInfo>> GetModulesListByPlaceIdAsync(int placeId, bool includeModulePermissions = false)
        {
            IQueryable<Module> query = _dbContext.KastraModules;

            if (includeModulePermissions)
            {
                query = query
                    .Include(m => m.ModuleDefinition.ModuleControls)
                    .Include(m => m.ModulePermissions)
                    .ThenInclude(m => m.Permission);
            }

            return await query
                .Where(m => m.PlaceId == placeId)
                .Select(m => m.ToModuleInfo(includeModulePermissions))
                .ToListAsync();
        }

        /// <inheritdoc cref="IViewManager.GetModulesListByPageIdAsync(int, bool)" />
        public async Task<IList<ModuleInfo>> GetModulesListByPageIdAsync(int pageId, bool includeModulePermissions = false)
        {
            IQueryable<Module> query = _dbContext.KastraModules;

            if (includeModulePermissions)
            {
                query = query
                    .Include(m => m.ModuleDefinition.ModuleControls)
                    .Include(m => m.ModulePermissions)
                    .ThenInclude(m => m.Permission);
            }

            return await query
                .Where(m => m.PageId == pageId)
                .Select(m => m.ToModuleInfo(includeModulePermissions))
                .ToListAsync();
        }

        /// <inheritdoc cref="IViewManager.SaveModuleAsync(ModuleInfo)" />
        public async Task<bool> SaveModuleAsync(ModuleInfo module)
        {
            Module newModule;

            if (module is null)
            {
                return false;
            }

            newModule = module.ToModule();

            if (module.ModuleId > 0)
            {
                _dbContext.KastraModules.Update(newModule);
            }
            else
            {
                _dbContext.KastraModules.Add(newModule);
            }

            await _dbContext.SaveChangesAsync();

            //Get module id
            module.ModuleId = newModule.ModuleId;

            // Clear cache
            _cacheEngine.ClearCacheContains("Module");
            _cacheEngine.ClearCacheContains(string.Format(PageConfiguration.PageByKeyCacheKey, string.Empty));

            return true;
        }

        /// <inheritdoc cref="IViewManager.DeleteModuleAsync(int)" />
        public async Task<bool> DeleteModuleAsync(int moduleId)
        {
            if (moduleId < 1)
            {
                return false;
            }

            Module module = await _dbContext.KastraModules
                .Include(m => m.ModulePermissions)
                .Include(m => m.Place)
                .SingleOrDefaultAsync(p => p.ModuleId == moduleId);

            if (module is null)
            {
                return false;
            }

            // Delete all permissions
            foreach(ModulePermission permission in module.ModulePermissions)
            {
                _dbContext.KastraModulePermissions.Remove(permission);
            }

            _dbContext.KastraModules.Remove(module);

            await _dbContext.SaveChangesAsync();

            // Clear cache
            _cacheEngine.ClearCacheContains("Module");
            _cacheEngine.ClearCacheContains(string.Format(PageConfiguration.PageByKeyCacheKey, string.Empty));

            return true;
        }

        #endregion

        #region Module control

        /// <inheritdoc cref="IViewManager.GetModuleControlAsync(int)" />
        public async Task<ModuleControlInfo> GetModuleControlAsync(int moduleControlId)
        {
            return (await _dbContext.KastraModuleControls
                .SingleOrDefaultAsync(mc => mc.ModuleControlId == moduleControlId))
                .ToModuleControlInfo();
        }

        /// <inheritdoc cref="IViewManager.GetModuleControlsListAsync(int)" />
        public async Task<IList<ModuleControlInfo>> GetModuleControlsListAsync(int moduleDefinitionId)
        {
            return await _dbContext.KastraModuleControls
                .Where(mc => mc.ModuleDefinitionId == moduleDefinitionId)
                .Select(mc => mc.ToModuleControlInfo())
                .ToListAsync();
        }

        /// <inheritdoc cref="IViewManager.SaveModuleControlAsync(ModuleControlInfo)" />
        public async Task<bool> SaveModuleControlAsync(ModuleControlInfo moduleControlInfo)
        {
            if (moduleControlInfo is null)
            {
                return false;
            }

            ModuleControl moduleControl = await _dbContext.KastraModuleControls
                .SingleOrDefaultAsync(mc => mc.ModuleControlId == moduleControlInfo.ModuleControlId);

            ModuleDefinitionInfo moduleDef = await GetModuleDefAsync(moduleControlInfo.ModuleDefId);

            if (moduleDef is null)
            {
                return false;
            }

            moduleControl = moduleControlInfo.ToModuleControl();

            if (moduleControl.ModuleControlId > 0)
            {
                _dbContext.KastraModuleControls.Update(moduleControl);
            }
            else
            {
                _dbContext.KastraModuleControls.Add(moduleControl);
            }

            await _dbContext.SaveChangesAsync();

            // Clear cache
            _cacheEngine.ClearCacheContains("Module");

            return true;
        }

        /// <inheritdoc cref="IViewManager.DeleteModuleControlAsync(int)" />
        public async Task<bool> DeleteModuleControlAsync(int moduleControlId)
        {
            if (moduleControlId < 1)
            {
                return false;
            }

            ModuleControl moduleControl = await _dbContext.KastraModuleControls
                .SingleOrDefaultAsync(p => p.ModuleControlId == moduleControlId);

            if (moduleControl is null)
            {
                return false;
            }

            _dbContext.KastraModuleControls.Remove(moduleControl);

            await _dbContext.SaveChangesAsync();

            // Clear cache
            _cacheEngine.ClearCacheContains("Module");

            return true;
        }

        #endregion

        #region Module Navigation

        /// <inheritdoc cref="IViewManager.GetModuleNavigationListAsync(int)" />
        public async Task<IList<ModuleNavigationInfo>> GetModuleNavigationListAsync(int moduleDefinitionId)
        {
            return await _dbContext.KastraModuleNavigations
                .Where(mc => mc.ModuleDefinitionId == moduleDefinitionId)
                .Select(mc => mc.ToModuleNavigationInfo())
                .ToListAsync();
        }

        /// <inheritdoc cref="IViewManager.GetModuleNavigationListByTypeAsync(string)" />
        public async Task<IList<ModuleNavigationInfo>> GetModuleNavigationListByTypeAsync(string type)
        {
            return await _dbContext.KastraModuleNavigations
                .Where(mc => mc.Type == type)
                .Select(mc => mc.ToModuleNavigationInfo())
                .ToListAsync();
        }

        /// <inheritdoc cref="IViewManager.SaveModuleNavigationAsync(ModuleNavigationInfo)" />
        public async Task<bool> SaveModuleNavigationAsync(ModuleNavigationInfo moduleNavigation)
        {
            if (moduleNavigation is null)
            {
                return false;
            }

            ModuleDefinitionInfo moduleDef = await GetModuleDefAsync(moduleNavigation.ModuleDefinitionId);

            if (moduleDef is null)
            {
                return false;
            }

            ModuleNavigation navigation = moduleNavigation.Id > 0 ? 
                await _dbContext.KastraModuleNavigations.SingleOrDefaultAsync(mn => mn.Id == moduleNavigation.Id) : null;

            navigation = moduleNavigation.ToModuleNavigation();

            if (navigation.Id > 0)
            {
                _dbContext.KastraModuleNavigations.Update(navigation);
            }
            else
            {
                _dbContext.KastraModuleNavigations.Add(navigation);
            }

            await _dbContext.SaveChangesAsync();

            // Clear cache
            _cacheEngine.ClearCacheContains("Module");

            return true;
        }

        /// <inheritdoc cref="IViewManager.DeleteModuleNavigationAsync(int)" />
        public async Task<bool> DeleteModuleNavigationAsync(int moduleNavigationId)
        {
            if (moduleNavigationId < 1)
            {
                return false;
            }

            ModuleNavigation moduleNavigation = await _dbContext.KastraModuleNavigations
                .SingleOrDefaultAsync(p => p.Id == moduleNavigationId);

            if (moduleNavigation is null)
            {
                return false;
            }

            _dbContext.KastraModuleNavigations.Remove(moduleNavigation);

            await _dbContext.SaveChangesAsync();

            // Clear cache
            _cacheEngine.ClearCacheContains("Module");

            return true;
        }

        #endregion
    }
}