using Kastra.Core.DTO;
using Kastra.DAL.EntityFramework.Models;

namespace Kastra.Business.Mappers
{
    public static class ModuleControlMapper
	{
		/// <summary>
		/// Convert ModuleControl to ModuleControleInfo.
		/// </summary>
		/// <param name="moduleControl">Module control</param>
		/// <returns>Module control info</returns>
		public static ModuleControlInfo ToModuleControlInfo(this ModuleControl moduleControl)
		{
			if (moduleControl is null)
			{
				return null;
			}

			return new ModuleControlInfo()
			{
				ModuleControlId = moduleControl.ModuleControlId,
				KeyName = moduleControl.KeyName,
				ModuleDefId = moduleControl.ModuleDefinitionId,
				Path = moduleControl.Path,
				ModuleDefinition = moduleControl.ModuleDefinition.ToModuleDefinitionInfo()
			};
		}

		/// <summary>
		/// Convert ModuleControlInfo to ModuleControl.
		/// </summary>
		/// <param name="moduleControlInfo">Module control info</param>
		/// <returns>Module control</returns>
		public static ModuleControl ToModuleControl(this ModuleControlInfo moduleControlInfo)
		{
			if (moduleControlInfo is null)
            {
				return null;
            }

			return new ModuleControl()
			{
				ModuleControlId = moduleControlInfo.ModuleControlId,
				ModuleDefinitionId = moduleControlInfo.ModuleDefId,
				KeyName = moduleControlInfo.KeyName,
				Path = moduleControlInfo.Path
			};
		}
	}
}
