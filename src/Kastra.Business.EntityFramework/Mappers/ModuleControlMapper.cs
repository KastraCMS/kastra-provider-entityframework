using System;
using Kastra.Core;
using Kastra.Core.Dto;
using Kastra.DAL.EntityFramework.Models;

namespace Kastra.Business.Mappers
{
	public static class ModuleControlMapper
	{
		public static ModuleControlInfo ToModuleControlInfo(this KastraModuleControls moduleControl)
		{
			if (moduleControl == null)
				return null;
            
			ModuleControlInfo moduleControlInfo = new ModuleControlInfo();
			moduleControlInfo.ModuleControlId = moduleControl.ModuleControlId;
			moduleControlInfo.KeyName = moduleControl.KeyName;
			moduleControlInfo.ModuleDefId = moduleControl.ModuleDefId;
			moduleControlInfo.Path = moduleControl.Path;

            if(moduleControl.ModuleDef != null)
			    moduleControlInfo.ModuleDefinition = ModuleDefinitionMapper.ToModuleDefinitionInfo(moduleControl.ModuleDef);

			return moduleControlInfo;
		}

		public static KastraModuleControls ToKastraModuleControl(this ModuleControlInfo moduleControlInfo)
		{
			KastraModuleControls moduleControl = new KastraModuleControls();
			moduleControl.ModuleControlId = moduleControlInfo.ModuleControlId;
			moduleControl.ModuleDefId = moduleControlInfo.ModuleDefId;
			moduleControl.KeyName = moduleControlInfo.KeyName;
			moduleControl.Path = moduleControlInfo.Path;

			return moduleControl;
		}
	}
}
