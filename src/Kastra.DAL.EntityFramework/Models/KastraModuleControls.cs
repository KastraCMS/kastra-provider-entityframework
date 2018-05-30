using System;
using System.Collections.Generic;

namespace Kastra.DAL.EntityFramework.Models
{
    public partial class KastraModuleControls
    {
        public int ModuleControlId { get; set; }
        public int ModuleDefId { get; set; }
        public string KeyName { get; set; }
        public string Path { get; set; }

        public virtual KastraModuleDefinitions ModuleDef { get; set; }
    }
}
