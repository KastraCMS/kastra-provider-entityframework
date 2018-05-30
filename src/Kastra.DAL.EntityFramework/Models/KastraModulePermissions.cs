using System;
using System.Collections.Generic;

namespace Kastra.DAL.EntityFramework.Models
{
    public partial class KastraModulePermissions
    {
        public int ModulePermissionId { get; set; }
        public int PermissionId { get; set; }
        public int ModuleId { get; set; }

        public virtual KastraModules Module { get; set; }
        public virtual KastraPermissions Permission { get; set; }
    }
}
