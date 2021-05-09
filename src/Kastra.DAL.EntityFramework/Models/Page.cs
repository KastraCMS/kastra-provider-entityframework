namespace Kastra.DAL.EntityFramework.Models
{
    public partial class Page
    {
        public int PageId { get; set; }
        public int PageTemplateId { get; set; }
        public string Title { get; set; }
        public string KeyName { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaRobot { get; set; }

        public virtual PageTemplate PageTemplate { get; set; }
    }
}
