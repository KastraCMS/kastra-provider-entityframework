namespace Kastra.DAL.EntityFramework.Models
{
    public partial class ModulePermission
    {
        public int ModulePermissionId { get; set; }
        public int PermissionId { get; set; }
        public int ModuleId { get; set; }

        public virtual Module Module { get; set; }
        public virtual Permission Permission { get; set; }
    }
}
