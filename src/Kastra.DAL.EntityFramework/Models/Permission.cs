using System.Collections.Generic;

namespace Kastra.DAL.EntityFramework.Models
{
    public partial class Permission
    {
        public Permission()
        {
            KastraModulePermissions = new HashSet<ModulePermission>();
        }

        public int PermissionId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ModulePermission> KastraModulePermissions { get; set; }
    }
}
