using System.Linq;
using Kastra.Core.DTO;
using Kastra.DAL.EntityFramework.Models;

namespace Kastra.Business.Mappers
{
    public static class ModuleDefinitionMapper
	{
		/// <summary>
		/// Convert ModuleDefinition to ModuleDefinitionInfo.
		/// </summary>
		/// <param name="moduleDefinition">Module definition</param>
		/// <param name="includeModules">Convert module list</param>
		/// <param name="includeModuleControls">Convert module control list</param>
		/// <returns>Module definition info</returns>
		public static ModuleDefinitionInfo ToModuleDefinitionInfo(
			this ModuleDefinition moduleDefinition, 
			bool includeModules = false, 
			bool includeModuleControls = false)
		{
			if (moduleDefinition is null)
            {
				return null;
            }

			var moduleDefinitionInfo = new ModuleDefinitionInfo()
			{
				ModuleDefId = moduleDefinition.ModuleDefId,
				KeyName = moduleDefinition.KeyName,
				Name = moduleDefinition.Name,
				Path = moduleDefinition.Path,
				Namespace = moduleDefinition.Namespace,
				Version = moduleDefinition.Version
			};

            if(includeModuleControls && moduleDefinition.ModuleControls is not null)
            {
				moduleDefinitionInfo.ModuleControls = moduleDefinition.ModuleControls
					.Select(mc => mc.ToModuleControlInfo())
					.ToList();
            }

            if(includeModules && moduleDefinition.Modules is not null)
            {
				moduleDefinitionInfo.Modules = moduleDefinition.Modules
					.Select(m => m.ToModuleInfo())
					.ToList();
            }
            
			return moduleDefinitionInfo;
		}

		/// <summary>
		/// Convert ModuleDefinitionInfo to ModuleDefinition.
		/// </summary>
		/// <param name="moduleDefinitionInfo">Module definition info</param>
		/// <returns>Module definition</returns>
		public static ModuleDefinition ToModuleDefinition(this ModuleDefinitionInfo moduleDefinitionInfo)
		{
			if (moduleDefinitionInfo is null)
            {
				return null;
            }

			return new ModuleDefinition()
			{
				ModuleDefId = moduleDefinitionInfo.ModuleDefId,
				KeyName = moduleDefinitionInfo.KeyName,
				Name = moduleDefinitionInfo.Name,
				Path = moduleDefinitionInfo.Path,
				Namespace = moduleDefinitionInfo.Namespace,
				Version = moduleDefinitionInfo.Version
			};
		}
	}
}
