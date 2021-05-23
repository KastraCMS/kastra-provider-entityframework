namespace Kastra.DAL.EntityFramework.Models
{
    public partial class ModuleNavigation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }
        public int ModuleDefinitionId { get; set; }

        public virtual ModuleDefinition ModuleDefinition { get; set; }
    }
}
