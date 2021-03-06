﻿using System.Collections.Generic;
using System.Linq;
using Kastra.Business.Mappers;
using Kastra.Core.Services.Contracts;
using Kastra.Core.Constants;
using Kastra.Core.DTO;
using Kastra.Core.Services;
using Kastra.DAL.EntityFramework;
using Kastra.DAL.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;

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

        public IList<PageInfo> GetPagesList()
        {
            if (_dbContext is null)
            {
                return null;
            }

            return _dbContext.KastraPages
                .Select(p => p.ToPageInfo(false, false))
                .ToList(); ;
        }

        public bool SavePage(PageInfo page)
        {
            if (page == null)
                return false;

            Page currentPage =  _dbContext.KastraPages.SingleOrDefault(p => p.PageId == page.PageId);

            if(currentPage != null)
            {
                if (currentPage.PageTemplateId != page.PageTemplateId)
                {
                    // Get the modules of the page
                    List<Module> modules = _dbContext.KastraModules
                                                    .Where(m => m.PageId == currentPage.PageId)
                                                    .ToList();

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

            _dbContext.SaveChanges();

            // Clear cache
            _cacheEngine.ClearCacheContains("Page");

            return true;
        }

        public PageInfo GetPage(int pageID, bool getAll = false)
        {
            Page page = null;
            string pageCacheKey = string.Format(PageConfiguration.PageCacheKey, pageID);

            if (_cacheEngine.GetCacheObject(pageCacheKey, out PageInfo pageInfo))
            {
                return pageInfo;
            }

            if (getAll)
            {
                page = _dbContext.KastraPages.Include(p => p.PageTemplate.KastraPlaces)
                              .SingleOrDefault(p => p.PageId == pageID);

                if (page == null)
                    return null;

                pageInfo = page.ToPageInfo(true, true);

                foreach (var place in pageInfo.PageTemplate.Places)
                {
                    if(place.ModuleId.HasValue)
                    {
                        place.StaticModule = GetModule(place.ModuleId.Value, true, false);
                    }
                    
                    place.Modules = GetModulesListByPlaceId(place.PlaceId, true);
                }
                    
            }
            else
            {
                page = _dbContext.KastraPages.SingleOrDefault(p => p.PageId == pageID);

                if (page is null)
                {
                    return null;
                }

                pageInfo = page.ToPageInfo();
            }

            _cacheEngine.SetCacheObject<PageInfo>(pageCacheKey, pageInfo);

            return pageInfo;
        }

        public PageInfo GetPageByKey(string keyName, bool getAll = false)
        {
            Page page = null;
            string pageCacheKey = string.Format(PageConfiguration.PageByKeyCacheKey, keyName);

            if (_cacheEngine.GetCacheObject(pageCacheKey, out PageInfo pageInfo))
            {
                return pageInfo;
            }

            if (getAll)
            {
                page = _dbContext.KastraPages
                    .Include(p => p.PageTemplate.KastraPlaces)
                    .SingleOrDefault(p => p.KeyName == keyName);

                if (page is null)
                {
                    return null;
                }

                pageInfo = page.ToPageInfo(true, true);

                foreach (var place in pageInfo.PageTemplate.Places)
                {
                    if(place.ModuleId.HasValue)
                    {
                        place.StaticModule = GetModule(place.ModuleId.Value, true, false);
                    }

                    place.Modules = GetModulesListByPlaceId(place.PlaceId, true);
                }
                    
            }
            else
            {
                page = _dbContext.KastraPages.SingleOrDefault(p => p.KeyName == keyName);

                if (page is null)
                {
                    return null;
                }

                pageInfo = page.ToPageInfo();
            }

            _cacheEngine.SetCacheObject(pageCacheKey, pageInfo);

            return pageInfo;
        }

        public bool DeletePage(int pageID)
        {
            if (pageID < 1)
                return false;

            Page page = _dbContext.KastraPages.SingleOrDefault(p => p.PageId == pageID);

            if (page == null)
                return false;

            _dbContext.KastraPages.Remove(page);
            _dbContext.SaveChanges();

            // Clear cache
            _cacheEngine.ClearCacheContains("Page");

            return true;
        }

        #endregion

        #region Page template

        public IList<TemplateInfo> GetPageTemplatesList()
        {
            if (_dbContext == null)
                return null;

            return _dbContext.KastraPageTemplates
                .Select(t => t.ToTemplateInfo(false, false))
                .ToList(); ;
        }

        public TemplateInfo GetPageTemplate(int pageTemplateID)
        {
            return _dbContext.KastraPageTemplates
                .SingleOrDefault(pt => pt.PageTemplateId == pageTemplateID)
                .ToTemplateInfo();
        }

        public bool SavePageTemplate(TemplateInfo templateInfo)
        {
            PageTemplate template = null;

            if (templateInfo is null)
            {
                return false;
            }

            if (templateInfo.TemplateId > 0)
            {
                template = _dbContext.KastraPageTemplates
                    .SingleOrDefault(pt => pt.PageTemplateId == templateInfo.TemplateId);
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

            _dbContext.SaveChanges();

            templateInfo.TemplateId = template.PageTemplateId;

            // Clear cache
            _cacheEngine.ClearCacheContains("Template");

            return true;
        }

        public bool DeletePageTemplate(int pageTemplateID)
        {
            if (pageTemplateID < 1)
            {
                return false;
            }

            PageTemplate pageTemplate = _dbContext.KastraPageTemplates
                .SingleOrDefault(p => p.PageTemplateId == pageTemplateID);

            if (pageTemplate is null)
            {
                return false;
            }

            _dbContext.KastraPageTemplates.Remove(pageTemplate);
            _dbContext.SaveChanges();

            // Clear cache
            _cacheEngine.ClearCacheContains("Template");

            return true;
        }

        #endregion

        #region Place

        public IList<PlaceInfo> GetPlacesList(bool includeModules = false)
        {
            return _dbContext.KastraPlaces
                .Include(p => p.KastraModules)
                .Select(p => p.ToPlaceInfo(includeModules))
                .ToList();
        }

        public PlaceInfo GetPlace(int placeID)
        {
            return _dbContext.KastraPlaces
                .SingleOrDefault(pt => pt.PlaceId == placeID)
                .ToPlaceInfo();
        }

        public bool SavePlace(PlaceInfo place)
        {
            if (place is null)
            {
                return false;
            }

            Place currentPlace = _dbContext.KastraPlaces
                .SingleOrDefault(p => p.PlaceId == place.PlaceId);

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

            _dbContext.SaveChanges();

            // Clear cache
            _cacheEngine.ClearCacheContains("Place");

            return true;
        }

        public bool DeletePlace(int placeID)
        {
            if (placeID < 1)
            {
                return false;
            }

            Place place = _dbContext.KastraPlaces
                .SingleOrDefault(p => p.PlaceId == placeID);

            if (place is null)
            {
                return false;
            }

            _dbContext.KastraPlaces.Remove(place);
            _dbContext.SaveChanges();

            // Clear cache
            _cacheEngine.ClearCacheContains("Place");

            return true;
        }

        #endregion

        #region Module Definition

        public ModuleDefinitionInfo GetModuleDef(int moduleDefID, bool getModuleControls = false)
        {
            ModuleDefinition moduleDefinition = null;

            if (getModuleControls)
            {
                moduleDefinition = _dbContext.KastraModuleDefinitions.Include(md => md.ModuleControls).SingleOrDefault(pt => pt.ModuleDefId == moduleDefID);
            }
            else
            {
                moduleDefinition = _dbContext.KastraModuleDefinitions.SingleOrDefault(pt => pt.ModuleDefId == moduleDefID);
            }

            return moduleDefinition.ToModuleDefinitionInfo(true, true);
        }

        public IList<ModuleDefinitionInfo> GetModuleDefsList()
        {
            return _dbContext.KastraModuleDefinitions
                .Select(md => md.ToModuleDefinitionInfo(false, false))
                .ToList();
        }

        public bool SaveModuleDef(ModuleDefinitionInfo moduleDefinition)
        {
            ModuleDefinition newModuleDefinition = null;

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

            _dbContext.SaveChanges();

            // Clear cache
            _cacheEngine.ClearCacheContains("Module");

            return true;
        }

        public bool DeleteModuleDef(int moduleDefinitionId)
        {
            if (moduleDefinitionId < 1)
            {
                return false;
            }

            ModuleDefinition moduleDef = _dbContext.KastraModuleDefinitions
                .SingleOrDefault(p => p.ModuleDefId == moduleDefinitionId);

            if (moduleDef is null)
            {
                return false;
            }

            _dbContext.KastraModuleDefinitions.Remove(moduleDef);
            _dbContext.SaveChanges();

            // Clear cache
            _cacheEngine.ClearCacheContains("Module");

            return true;
        }

        #endregion

        #region Module

        public ModuleInfo GetModule(int moduleID, bool includeModuleDef = false, bool includePlace = false)
        {
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

            return query
                .SingleOrDefault(pt => pt.ModuleId == moduleID)
                .ToModuleInfo(true);
        }

        public IList<ModuleInfo> GetModulesList(bool includeModuleControls = false)
        {
            IList<Module> modulesList = null;

            if (includeModuleControls)
            {
                modulesList = _dbContext.KastraModules
                    .Include(m => m.ModuleDefinition.ModuleControls)
                    .ToList();
            }
            else
            {
                modulesList = _dbContext.KastraModules.ToList();
            }

            return modulesList
                .Select(m => m.ToModuleInfo(false))
                .ToList();
        }

        public IList<ModuleInfo> GetModulesListByPlaceId(int placeId, bool includeModulePermissions = false)
        {
            IQueryable<Module> query = _dbContext.KastraModules;

            if (includeModulePermissions)
            {
                query = query
                    .Include(m => m.ModuleDefinition.ModuleControls)
                    .Include(m => m.ModulePermissions)
                    .ThenInclude(m => m.Permission);
            }

            return query
                .Where(m => m.PlaceId == placeId)
                .Select(m => m.ToModuleInfo(includeModulePermissions))
                .ToList();
        }

        public IList<ModuleInfo> GetModulesListByPageId(int pageId, bool includeModulePermissions = false)
        {
            IQueryable<Module> query = _dbContext.KastraModules;

            if (includeModulePermissions)
            {
                query = query
                    .Include(m => m.ModuleDefinition.ModuleControls)
                    .Include(m => m.ModulePermissions)
                    .ThenInclude(m => m.Permission);
            }

            return query
                .Where(m => m.PageId == pageId)
                .Select(m => m.ToModuleInfo(includeModulePermissions))
                .ToList();
        }

        public bool SaveModule(ModuleInfo module)
        {
            Module newModule = null;

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

            _dbContext.SaveChanges();

            //Get module id
            module.ModuleId = newModule.ModuleId;

            // Clear cache
            _cacheEngine.ClearCacheContains("Module");
            _cacheEngine.ClearCacheContains(string.Format(PageConfiguration.PageByKeyCacheKey, string.Empty));

            return true;
        }

        public bool DeleteModule(int moduleId)
        {
            if (moduleId < 1)
            {
                return false;
            }

            Module module = _dbContext.KastraModules
                .Include(m => m.ModulePermissions)
                .Include(m => m.Place)
                .SingleOrDefault(p => p.ModuleId == moduleId);

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
            _dbContext.SaveChanges();

            // Clear cache
            _cacheEngine.ClearCacheContains("Module");
            _cacheEngine.ClearCacheContains(string.Format(PageConfiguration.PageByKeyCacheKey, string.Empty));

            return true;
        }

        #endregion

        #region Module control

        public ModuleControlInfo GetModuleControl(int moduleControlId)
        {
            return _dbContext.KastraModuleControls
                .SingleOrDefault(mc => mc.ModuleControlId == moduleControlId)
                .ToModuleControlInfo();
        }

        public IList<ModuleControlInfo> GetModuleControlsList(int moduleDefinitionId)
        {
            return _dbContext.KastraModuleControls
                .Where(mc => mc.ModuleDefinitionId == moduleDefinitionId)
                .Select(mc => mc.ToModuleControlInfo())
                .ToList();
        }

        public bool SaveModuleControl(ModuleControlInfo moduleControlInfo)
        {
            if (moduleControlInfo is null)
            {
                return false;
            }

            ModuleControl moduleControl = _dbContext.KastraModuleControls
                .SingleOrDefault(mc => mc.ModuleControlId == moduleControlInfo.ModuleControlId);

            ModuleDefinitionInfo moduleDef = GetModuleDef(moduleControlInfo.ModuleDefId);

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

            _dbContext.SaveChanges();

            // Clear cache
            _cacheEngine.ClearCacheContains("Module");

            return true;
        }

        public bool DeleteModuleControl(int moduleControlId)
        {
            if (moduleControlId < 1)
            {
                return false;
            }

            ModuleControl moduleControl = _dbContext.KastraModuleControls
                .SingleOrDefault(p => p.ModuleControlId == moduleControlId);

            if (moduleControl is null)
            {
                return false;
            }

            _dbContext.KastraModuleControls.Remove(moduleControl);
            _dbContext.SaveChanges();

            // Clear cache
            _cacheEngine.ClearCacheContains("Module");

            return true;
        }

        #endregion

        #region Module Navigation

        public IList<ModuleNavigationInfo> GetModuleNavigationList(int moduleDefinitionId)
        {
            return _dbContext.KastraModuleNavigations
                .Where(mc => mc.ModuleDefinitionId == moduleDefinitionId)
                .Select(mc => mc.ToModuleNavigationInfo())
                .ToList();
        }

        public IList<ModuleNavigationInfo> GetModuleNavigationListByType(string type)
        {
            return _dbContext.KastraModuleNavigations
                .Where(mc => mc.Type == type)
                .Select(mc => mc.ToModuleNavigationInfo())
                .ToList();
        }

        public bool SaveModuleNavigation(ModuleNavigationInfo moduleNavigation)
        {
            if (moduleNavigation is null)
            {
                return false;
            }

            ModuleDefinitionInfo moduleDef = GetModuleDef(moduleNavigation.ModuleDefinitionId);

            if (moduleDef is null)
            {
                return false;
            }

            ModuleNavigation navigation = moduleNavigation.Id > 0 ? 
                _dbContext.KastraModuleNavigations.SingleOrDefault(mn => mn.Id == moduleNavigation.Id) : null;

            navigation = moduleNavigation.ToModuleNavigation();

            if (navigation.Id > 0)
            {
                _dbContext.KastraModuleNavigations.Update(navigation);
            }
            else
            {
                _dbContext.KastraModuleNavigations.Add(navigation);
            }

            _dbContext.SaveChanges();

            // Clear cache
            _cacheEngine.ClearCacheContains("Module");

            return true;
        }

        public bool DeleteModuleNavigation(int moduleNavigationId)
        {
            if (moduleNavigationId < 1)
            {
                return false;
            }

            ModuleNavigation moduleNavigation = _dbContext.KastraModuleNavigations
                .SingleOrDefault(p => p.Id == moduleNavigationId);

            if (moduleNavigation is null)
            {
                return false;
            }

            _dbContext.KastraModuleNavigations.Remove(moduleNavigation);
            _dbContext.SaveChanges();

            // Clear cache
            _cacheEngine.ClearCacheContains("Module");

            return true;
        }

        #endregion
    }
}