using System;
using System.Linq;
using Kastra.Core;
using Kastra.Core.Dto;
using Kastra.DAL.EntityFramework.Models;

namespace Kastra.Business.Mappers
{
	public static class ModuleDefinitionMapper
	{
		public static ModuleDefinitionInfo ToModuleDefinitionInfo(this KastraModuleDefinitions moduleDefinition, Boolean includeModules = false, Boolean includeModuleControls = false)
		{
			ModuleDefinitionInfo moduleDefinitionInfo = new ModuleDefinitionInfo();
			moduleDefinitionInfo.ModuleDefId = moduleDefinition.ModuleDefId;
			moduleDefinitionInfo.KeyName = moduleDefinition.KeyName;
			moduleDefinitionInfo.Name = moduleDefinition.Name;
			moduleDefinitionInfo.Path = moduleDefinition.Path;
            moduleDefinitionInfo.Namespace = moduleDefinition.Namespace;
			moduleDefinitionInfo.Version = moduleDefinition.Version;

            if(includeModuleControls)
				moduleDefinitionInfo.ModuleControls = moduleDefinition.KastraModuleControls.Select(mc => ModuleControlMapper.ToModuleControlInfo(mc)).ToList();

            if(includeModules)
				moduleDefinitionInfo.Modules = moduleDefinition.KastraModules.Select(m => ModuleMapper.ToModuleInfo(m)).ToList();
            
			return moduleDefinitionInfo;
		}

		public static KastraModuleDefinitions ToKastraModuleDefinition(this ModuleDefinitionInfo moduleDefinitionInfo)
		{
			KastraModuleDefinitions moduleDefinition = new KastraModuleDefinitions();
			moduleDefinition.ModuleDefId = moduleDefinitionInfo.ModuleDefId;
			moduleDefinition.KeyName = moduleDefinitionInfo.KeyName;
			moduleDefinition.Name = moduleDefinitionInfo.Name;
			moduleDefinition.Path = moduleDefinitionInfo.Path;
            moduleDefinition.Namespace = moduleDefinitionInfo.Namespace;
			moduleDefinition.Version = moduleDefinitionInfo.Version;

			return moduleDefinition;
		}
	}
}
