using System.Collections.Generic;

namespace Kastra.DAL.EntityFramework.Models
{
    public partial class KastraModules
    {
        public KastraModules()
        {
            KastraModulePermissions = new HashSet<KastraModulePermissions>();
        }

        public int ModuleId { get; set; }
        public int ModuleDefId { get; set; }
        public int PlaceId { get; set; }
        public int PageId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<KastraModulePermissions> KastraModulePermissions { get; set; }
        public virtual KastraModuleDefinitions ModuleDef { get; set; }
        public virtual KastraPlaces Place { get; set; }
    }
}
