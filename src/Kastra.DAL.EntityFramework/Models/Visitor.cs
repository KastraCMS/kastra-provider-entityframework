using Kastra.Core.Identity;
using System;

namespace Kastra.DAL.EntityFramework.Models
{
    public class Visitor
    {
        public Guid Id { get; set; }
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
        public Guid? UserId { get; set; }
        public DateTime LastVisitAt { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
