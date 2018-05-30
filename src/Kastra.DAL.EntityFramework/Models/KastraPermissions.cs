using System.Collections.Generic;

namespace Kastra.DAL.EntityFramework.Models
{
    public partial class KastraPermissions
    {
        public KastraPermissions()
        {
            KastraModulePermissions = new HashSet<KastraModulePermissions>();
        }

        public int PermissionId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<KastraModulePermissions> KastraModulePermissions { get; set; }
    }
}
