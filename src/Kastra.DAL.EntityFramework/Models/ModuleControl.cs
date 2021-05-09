namespace Kastra.DAL.EntityFramework.Models
{
    public partial class ModuleControl
    {
        public int ModuleControlId { get; set; }
        public int ModuleDefinitionId { get; set; }
        public string KeyName { get; set; }
        public string Path { get; set; }

        public virtual ModuleDefinition ModuleDefinition { get; set; }
    }
}
