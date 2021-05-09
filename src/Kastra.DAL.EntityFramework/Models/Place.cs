using System.Collections.Generic;

namespace Kastra.DAL.EntityFramework.Models
{
    public partial class Place
    {
        public Place()
        {
            KastraModules = new HashSet<Module>();
        }

        public int PlaceId { get; set; }
        public int PageTemplateId { get; set; }
        public string KeyName { get; set; }
        public int? ModuleId { get; set; }

        public virtual ICollection<Module> KastraModules { get; set; }
        public virtual PageTemplate PageTemplate { get; set; }
        public virtual Module StaticKastraModule { get; set; }
    }
}
