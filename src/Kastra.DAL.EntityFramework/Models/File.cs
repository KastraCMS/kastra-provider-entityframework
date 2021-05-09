using System;

namespace Kastra.DAL.EntityFramework.Models
{
    public partial class File
    {
        public Guid FileId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public DateTime DateCreated { get; set; }
    }
}