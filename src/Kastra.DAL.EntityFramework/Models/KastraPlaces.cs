using System;
using System.Collections.Generic;

namespace Kastra.DAL.EntityFramework.Models
{
    public partial class KastraPlaces
    {
        public KastraPlaces()
        {
            KastraModules = new HashSet<KastraModules>();
        }

        public int PlaceId { get; set; }
        public int PageTemplateId { get; set; }
        public string KeyName { get; set; }

        public virtual ICollection<KastraModules> KastraModules { get; set; }
        public virtual KastraPageTemplates PageTemplate { get; set; }
    }
}
