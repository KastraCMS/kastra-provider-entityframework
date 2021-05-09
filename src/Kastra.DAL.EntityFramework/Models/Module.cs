using System.Collections.Generic;

namespace Kastra.DAL.EntityFramework.Models
{
    public partial class Module
    {
        public Module()
        {
            ModulePermissions = new HashSet<ModulePermission>();
        }

        public int ModuleId { get; set; }
        public int ModuleDefinitionId { get; set; }
        public int PlaceId { get; set; }
        public int PageId { get; set; }
        public string Name { get; set; }
        public bool IsDisabled { get; set; }

        public virtual ICollection<ModulePermission> ModulePermissions { get; set; }
        public virtual ModuleDefinition ModuleDefinition { get; set; }
        public virtual Place Place { get; set; }
        public virtual Place StaticPlace { get; set; }
    }
}
