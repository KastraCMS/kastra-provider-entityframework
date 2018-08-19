using Kastra.Core.DTO;
using Kastra.DAL.EntityFramework.Models;

namespace Kastra.Business.Mappers
{
    public static class VisitorMapper
    {
        public static VisitorInfo ToVisitorInfo(this KastraVisitors visitor)
        {
            VisitorInfo visitorInfo = new VisitorInfo();
            visitorInfo.Id = visitor.Id;
            visitorInfo.IpAddress = visitor.IpAddress;
            visitorInfo.LastVisitAt = visitor.LastVisitAt;
            visitorInfo.UserAgent = visitor.UserAgent;
            visitorInfo.UserId = visitor.UserId;

            return visitorInfo;
        }

        public static KastraVisitors ToKastraVisitor(this VisitorInfo visitorInfo)
        {
            KastraVisitors visitor = new KastraVisitors();
            visitor.Id = visitorInfo.Id;
            visitor.IpAddress = visitorInfo.IpAddress;
            visitor.LastVisitAt = visitorInfo.LastVisitAt;
            visitor.UserAgent = visitorInfo.UserAgent;
            visitor.UserId = visitorInfo.UserId;

            return visitor;
        }
    }
}
