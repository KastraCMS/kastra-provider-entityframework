using System;
using System.Collections.Generic;

namespace Kastra.DAL.EntityFramework.Models
{
    public partial class KastraPageTemplates
    {
        public KastraPageTemplates()
        {
            KastraPages = new HashSet<KastraPages>();
            KastraPlaces = new HashSet<KastraPlaces>();
        }

        public int PageTemplateId { get; set; }
        public string KeyName { get; set; }
        public string Name { get; set; }
        public string ViewPath { get; set; }
        public string ModelClass { get; set; }

        public virtual ICollection<KastraPages> KastraPages { get; set; }
        public virtual ICollection<KastraPlaces> KastraPlaces { get; set; }
    }
}
