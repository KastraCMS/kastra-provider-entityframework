using System.Collections.Generic;

namespace Kastra.DAL.EntityFramework.Models
{
    public partial class ModuleDefinition
    {
        public ModuleDefinition()
        {
            ModuleControls = new HashSet<ModuleControl>();
            Modules = new HashSet<Module>();
        }

        public int ModuleDefId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string KeyName { get; set; }
        public string Namespace { get; set; }
        public string Version { get; set; }

        public virtual ICollection<ModuleControl> ModuleControls { get; set; }
        public virtual ICollection<Module> Modules { get; set; }
        public virtual ICollection<ModuleNavigation> ModuleNavigations { get; set; } 
    }
}
