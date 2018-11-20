using System;
using System.Linq;
using Kastra.Core;
using Kastra.Core.Dto;
using Kastra.DAL.EntityFramework.Models;

namespace Kastra.Business.Mappers
{
	public static class ModuleMapper
	{
		public static ModuleInfo ToModuleInfo(this KastraModules module, Boolean includeChildren = false)
		{
			ModuleInfo moduleInfo = new ModuleInfo();
			moduleInfo.ModuleId = module.ModuleId;
			moduleInfo.Name = module.Name;
			moduleInfo.PageId = module.PageId;
			moduleInfo.PlaceId = module.PlaceId;
			moduleInfo.ModuleDefId = module.ModuleDefId;
			moduleInfo.IsDisabled = module.IsDisabled;

            if(module.ModuleDef != null)
			    moduleInfo.ModuleDefinition = ModuleDefinitionMapper.ToModuleDefinitionInfo(module.ModuleDef, false, true);

            if (module.Place != null)
			    moduleInfo.Place = PlaceMapper.ToPlaceInfo(module.Place);

            if(includeChildren)
			    moduleInfo.ModulePermissions = module.KastraModulePermissions.Select(mp => ModulePermissionMapper.ToModulePermissionInfo(mp, false))?.ToList();

			return moduleInfo;
		}

		public static KastraModules ToKastraModule(this ModuleInfo moduleInfo)
		{
			KastraModules module = new KastraModules();
			module.ModuleId = moduleInfo.ModuleId;
			module.ModuleDefId = moduleInfo.ModuleDefId;
			module.PageId = moduleInfo.PageId;
			module.PlaceId = moduleInfo.PlaceId;
			module.Name = moduleInfo.Name;
			module.IsDisabled = moduleInfo.IsDisabled;

			return module;
		}
	}
}
