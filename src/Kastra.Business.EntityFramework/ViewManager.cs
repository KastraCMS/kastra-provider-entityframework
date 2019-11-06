﻿using System.Collections.Generic;
using System.Linq;
using Kastra.Business.Mappers;
using Kastra.Core.Business;
using Kastra.Core.Constants;
using Kastra.Core.Dto;
using Kastra.Core.Services;
using Kastra.DAL.EntityFramework;
using Kastra.DAL.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace Kastra.Business
{
    public class ViewManager : IViewManager
    {
        #region Private members

        private readonly KastraContext _dbContext;
        private readonly CacheEngine _cacheEngine;

        #endregion

        public ViewManager(KastraContext dbContext, CacheEngine cacheEngine)
        {
            _dbContext = dbContext;
            _cacheEngine = cacheEngine;
        }

        #region Pages

        public IList<PageInfo> GetPagesList()
        {
            if (_dbContext == null)
                return null;

            IList<PageInfo> pagesList = _dbContext.KastraPages.Select(p => PageMapper.ToPageInfo(p, false)).ToList();

            return pagesList;
        }

        public bool SavePage(PageInfo page)
        {
            if (page == null)
                return false;

            KastraPages currentPage =  _dbContext.KastraPages.SingleOrDefault(p => p.PageId == page.PageId);

            if(currentPage != null)
            {
                if (currentPage.PageTemplateId != page.PageTemplateId)
                {
                    // Get the modules of the page
                    List<KastraModules> modules = _dbContext.KastraModules
                                                    .Where(m => m.PageId == currentPage.PageId)
                                                    .ToList();

                    // Remove 
                    foreach (KastraModules module in modules)
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
                _dbContext.KastraPages.Add(PageMapper.ToKastraPage(page));
            }

            _dbContext.SaveChanges();

            // Clear cache
            _cacheEngine.ClearCacheContains("Page");

            return true;
        }

        public PageInfo GetPage(int pageID, bool getAll = false)
        {
            PageInfo pageInfo = null;
            KastraPages page = null;
            string pageCacheKey = string.Format(PageConfiguration.PageCacheKey, pageID);

            if (_cacheEngine.GetCacheObject<PageInfo>(pageCacheKey, out pageInfo))
                return pageInfo;

            if (getAll)
            {
                page = _dbContext.KastraPages.Include(p => p.PageTemplate.KastraPlaces)
                              .SingleOrDefault(p => p.PageId == pageID);

                if (page == null)
                    return null;

                pageInfo = PageMapper.ToPageInfo(page, true);

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

                if (page == null)
                    return null;

                pageInfo = PageMapper.ToPageInfo(page);
            }

            _cacheEngine.SetCacheObject<PageInfo>(pageCacheKey, pageInfo);

            return pageInfo;
        }

        public PageInfo GetPageByKey(string keyName, bool getAll = false)
        {
            KastraPages page = null;
            PageInfo pageInfo = new PageInfo();
            string pageCacheKey = string.Format(PageConfiguration.PageByKeyCacheKey, keyName);

            if (_cacheEngine.GetCacheObject<PageInfo>(pageCacheKey, out pageInfo))
                return pageInfo;

            if (getAll)
            {
                page = _dbContext.KastraPages.Include(p => p.PageTemplate.KastraPlaces)
                                             .SingleOrDefault(p => p.KeyName == keyName);

                if (page == null)
                    return null;

                pageInfo = PageMapper.ToPageInfo(page, true);

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

                if (page == null)
                    return null;

                pageInfo = PageMapper.ToPageInfo(page);
            }

            _cacheEngine.SetCacheObject<PageInfo>(pageCacheKey, pageInfo);

            return pageInfo;
        }

        public bool DeletePage(int pageID)
        {
            if (pageID < 1)
                return false;

            KastraPages page = _dbContext.KastraPages.SingleOrDefault(p => p.PageId == pageID);

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

            IList<TemplateInfo> pageTemplatesList = _dbContext.KastraPageTemplates.Select(t => TemplateMapper.ToTemplateInfo(t, false)).ToList();

            return pageTemplatesList;
        }

        public TemplateInfo GetPageTemplate(int pageTemplateID)
        {
            KastraPageTemplates template = _dbContext.KastraPageTemplates.SingleOrDefault(pt => pt.PageTemplateId == pageTemplateID);

            if (template == null)
                return null;

            return TemplateMapper.ToTemplateInfo(template);
        }

        public bool SavePageTemplate(TemplateInfo templateInfo)
        {
            KastraPageTemplates template = null;

            if (templateInfo == null)
                return false;

            if (templateInfo.TemplateId > 0)
            {
                template = _dbContext.KastraPageTemplates.SingleOrDefault(pt => pt.PageTemplateId == templateInfo.TemplateId);
            }

            if (template == null)
            {
                template = new KastraPageTemplates();
            }

            template.Name = templateInfo.Name;
            template.KeyName = templateInfo.KeyName;
            template.ModelClass = templateInfo.ModelClass;
            template.ViewPath = templateInfo.ViewPath;

            if (templateInfo.TemplateId > 0)
                _dbContext.KastraPageTemplates.Update(template);
            else
                _dbContext.KastraPageTemplates.Add(template);

            _dbContext.SaveChanges();

            templateInfo.TemplateId = template.PageTemplateId;

            // Clear cache
            _cacheEngine.ClearCacheContains("Template");

            return true;
        }

        public bool DeletePageTemplate(int pageTemplateID)
        {
            if (pageTemplateID < 1)
                return false;

            KastraPageTemplates pageTemplate = _dbContext.KastraPageTemplates.SingleOrDefault(p => p.PageTemplateId == pageTemplateID);

            if (pageTemplate == null)
                return false;

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
            if (includeModules)
                return _dbContext.KastraPlaces.Include(p => p.KastraModules).Select(p => PlaceMapper.ToPlaceInfo(p, true)).ToList();
            else
                return _dbContext.KastraPlaces.Select(p => PlaceMapper.ToPlaceInfo(p, false)).ToList();

        }

        public PlaceInfo GetPlace(int placeID)
        {
            KastraPlaces place = _dbContext.KastraPlaces.SingleOrDefault(pt => pt.PlaceId == placeID);

            if (place == null)
                return null;

            return PlaceMapper.ToPlaceInfo(place);
        }

        public bool SavePlace(PlaceInfo place)
        {
            if (place == null)
                return false;

            KastraPlaces currentPlace = _dbContext.KastraPlaces.SingleOrDefault(p => p.PlaceId == place.PlaceId);

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
                _dbContext.KastraPlaces.Add(PlaceMapper.ToKastraPlace(place));
            }

            _dbContext.SaveChanges();

            // Clear cache
            _cacheEngine.ClearCacheContains("Place");

            return true;
        }

        public bool DeletePlace(int placeID)
        {
            if (placeID < 1)
                return false;

            KastraPlaces place = _dbContext.KastraPlaces.SingleOrDefault(p => p.PlaceId == placeID);

            if (place == null)
                return false;

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
            KastraModuleDefinitions moduleDefinition = null;

            if (getModuleControls)
                moduleDefinition = _dbContext.KastraModuleDefinitions.Include(md => md.KastraModuleControls).SingleOrDefault(pt => pt.ModuleDefId == moduleDefID);
            else
                moduleDefinition = _dbContext.KastraModuleDefinitions.SingleOrDefault(pt => pt.ModuleDefId == moduleDefID);

            if (moduleDefinition == null)
                return null;

            return ModuleDefinitionMapper.ToModuleDefinitionInfo(moduleDefinition, true, true);
        }

        public IList<ModuleDefinitionInfo> GetModuleDefsList()
        {
            return _dbContext.KastraModuleDefinitions.Select(md => ModuleDefinitionMapper.ToModuleDefinitionInfo(md, false, false)).ToList();
        }

        public bool SaveModuleDef(ModuleDefinitionInfo moduleDefinition)
        {
            KastraModuleDefinitions newModuleDefinition = null;

            if (moduleDefinition == null)
                return false;

            newModuleDefinition = ModuleDefinitionMapper.ToKastraModuleDefinition(moduleDefinition);

            if (newModuleDefinition.ModuleDefId > 0)
                _dbContext.KastraModuleDefinitions.Update(newModuleDefinition);
            else
                _dbContext.KastraModuleDefinitions.Add(newModuleDefinition);

            _dbContext.SaveChanges();

            // Clear cache
            _cacheEngine.ClearCacheContains("Module");

            return true;
        }

        public bool DeleteModuleDef(int moduleDefinitionId)
        {
            if (moduleDefinitionId < 1)
                return false;

            KastraModuleDefinitions moduleDef = _dbContext.KastraModuleDefinitions.SingleOrDefault(p => p.ModuleDefId == moduleDefinitionId);

            if (moduleDef == null)
                return false;

            _dbContext.KastraModuleDefinitions.Remove(moduleDef);
            _dbContext.SaveChanges();

            // Clear cache
            _cacheEngine.ClearCacheContains("Module");

            return true;
        }

        #endregion

        #region Module

        public ModuleInfo GetModule(int moduleID, bool getModuleDef = false, bool getPlace = false)
        {
            KastraModules module = null;
            IQueryable<KastraModules> query = _dbContext.KastraModules.Include(m => m.KastraModulePermissions)
                                                                        .ThenInclude(m => m.Permission);

            if (getModuleDef)
                query = query.Include(m => m.ModuleDef.KastraModuleControls);

            if (getPlace)
                query = query.Include(m => m.Place);

            module = query.SingleOrDefault(pt => pt.ModuleId == moduleID);

            if (module == null)
                return null;

            return ModuleMapper.ToModuleInfo(module, true);
        }

        public IList<ModuleInfo> GetModulesList(bool getModuleDefs = false)
        {
            IList<KastraModules> modulesList = null;

            if (getModuleDefs)
                modulesList = _dbContext.KastraModules.Include(m => m.ModuleDef.KastraModuleControls).ToList();
            else
                modulesList = _dbContext.KastraModules.ToList();

            return modulesList.Select(m => ModuleMapper.ToModuleInfo(m, false)).ToList();
        }

        public IList<ModuleInfo> GetModulesListByPlaceId(int placeId, bool getModuleDefs = false)
        {
            IList<KastraModules> modulesList = null;

            if (getModuleDefs)
                modulesList = _dbContext.KastraModules.Include(m => m.ModuleDef.KastraModuleControls)
                                        .Include(m => m.KastraModulePermissions)
                                        .ThenInclude(m => m.Permission)
                                        .Where(m => m.PlaceId == placeId).ToList();
            else
                modulesList = _dbContext.KastraModules.Where(m => m.PlaceId == placeId).ToList();

            return modulesList.Select(m => ModuleMapper.ToModuleInfo(m, true)).ToList();
        }

        public IList<ModuleInfo> GetModulesListByPageId(int pageId, bool getModuleDefs = false)
        {
            IList<KastraModules> modulesList = null;

            if (getModuleDefs)
                modulesList = _dbContext.KastraModules.Include(m => m.ModuleDef.KastraModuleControls)
                                        .Include(m => m.KastraModulePermissions)
                                        .ThenInclude(m => m.Permission)
                                        .Where(m => m.PageId == pageId).ToList();
            else
                modulesList = _dbContext.KastraModules.Where(m => m.PageId == pageId).ToList();

            return modulesList.Select(m => ModuleMapper.ToModuleInfo(m, false)).ToList();
        }

        public bool SaveModule(ModuleInfo module)
        {
            KastraModules newModule = null;

            if (module == null)
                return false;

            newModule = ModuleMapper.ToKastraModule(module);

            if (module.ModuleId > 0)
                _dbContext.KastraModules.Update(newModule);
            else
                _dbContext.KastraModules.Add(newModule);

            _dbContext.SaveChanges();

            //Get module id
            module.ModuleId = newModule.ModuleId;

            // Clear cache
            _cacheEngine.ClearCacheContains("Module");
            _cacheEngine.ClearCacheContains(string.Format(PageConfiguration.PageByKeyCacheKey, string.Empty));

            return true;
        }

        public bool DeleteModule(int moduleID)
        {
            if (moduleID < 1)
                return false;

            KastraModules module = _dbContext.KastraModules.SingleOrDefault(p => p.ModuleId == moduleID);

            if (module == null)
                return false;

            // Delete all permissions
            foreach(KastraModulePermissions permission in module.KastraModulePermissions)
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
            KastraModuleControls moduleControl = _dbContext.KastraModuleControls.SingleOrDefault(mc => mc.ModuleControlId == moduleControlId);

            if (moduleControl == null)
                return null;

            return ModuleControlMapper.ToModuleControlInfo(moduleControl);
        }

        public IList<ModuleControlInfo> GetModuleControlsList(int moduleDefId)
        {
            return _dbContext.KastraModuleControls.Where(mc => mc.ModuleDefId == moduleDefId).Select(mc => ModuleControlMapper.ToModuleControlInfo(mc)).ToList();
        }

        public bool SaveModuleControl(ModuleControlInfo moduleControlInfo)
        {
            KastraModuleControls moduleControl = _dbContext.KastraModuleControls
                                                           .SingleOrDefault(mc => mc.ModuleControlId == moduleControlInfo.ModuleControlId);
            ModuleDefinitionInfo moduleDef = null;

            moduleDef = GetModuleDef(moduleControlInfo.ModuleDefId);

            if (moduleDef == null)
                return false;

            moduleControl = ModuleControlMapper.ToKastraModuleControl(moduleControlInfo);

            if (moduleControl.ModuleControlId > 0)
                _dbContext.KastraModuleControls.Update(moduleControl);
            else
                _dbContext.KastraModuleControls.Add(moduleControl);

            _dbContext.SaveChanges();

            // Clear cache
            _cacheEngine.ClearCacheContains("Module");

            return true;
        }

        public bool DeleteModuleControl(int moduleControlId)
        {
            if (moduleControlId < 1)
                return false;

            KastraModuleControls moduleControl = _dbContext.KastraModuleControls.SingleOrDefault(p => p.ModuleControlId == moduleControlId);

            if (moduleControl == null)
                return false;

            _dbContext.KastraModuleControls.Remove(moduleControl);
            _dbContext.SaveChanges();

            // Clear cache
            _cacheEngine.ClearCacheContains("Module");

            return true;
        }


        #endregion
    }
}