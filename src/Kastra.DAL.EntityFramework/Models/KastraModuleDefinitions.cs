using System;
using System.Collections.Generic;

namespace Kastra.DAL.EntityFramework.Models
{
    public partial class KastraModuleDefinitions
    {
        public KastraModuleDefinitions()
        {
            KastraModuleControls = new HashSet<KastraModuleControls>();
            KastraModules = new HashSet<KastraModules>();
        }

        public int ModuleDefId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string KeyName { get; set; }
        public string Namespace { get; set; }
        public string Version { get; set; }

        public virtual ICollection<KastraModuleControls> KastraModuleControls { get; set; }
        public virtual ICollection<KastraModules> KastraModules { get; set; }
    }
}
