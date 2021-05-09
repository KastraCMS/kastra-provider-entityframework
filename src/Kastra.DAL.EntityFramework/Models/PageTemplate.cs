using System.Collections.Generic;

namespace Kastra.DAL.EntityFramework.Models
{
    public partial class PageTemplate
    {
        public PageTemplate()
        {
            KastraPages = new HashSet<Page>();
            KastraPlaces = new HashSet<Place>();
        }

        public int PageTemplateId { get; set; }
        public string KeyName { get; set; }
        public string Name { get; set; }
        public string ViewPath { get; set; }
        public string ModelClass { get; set; }

        public virtual ICollection<Page> KastraPages { get; set; }
        public virtual ICollection<Place> KastraPlaces { get; set; }
    }
}
