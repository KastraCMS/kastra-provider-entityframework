using System;
namespace Kastra.DAL.EntityFramework.Models
{
    public class KastraVisitors
    {
        public Guid Id { get; set; }
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
        public string UserId { get; set; }
        public DateTime LastVisitAt { get; set; }
    }
}
