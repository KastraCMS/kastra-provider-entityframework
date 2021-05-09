using Kastra.Core.DTO;
using Kastra.DAL.EntityFramework.Models;

namespace Kastra.Business.Mappers
{
    public static class VisitorMapper
    {
        /// <summary>
        /// Convert Visitor to VisitorInfo.
        /// </summary>
        /// <param name="visitor">Visitor</param>
        /// <returns>Visitor info</returns>
        public static VisitorInfo ToVisitorInfo(this Visitor visitor)
        {
            if (visitor is null)
            {
                return null;
            }

            return new VisitorInfo()
            { 
                Id = visitor.Id,
                IpAddress = visitor.IpAddress,
                LastVisitAt = visitor.LastVisitAt,
                UserAgent = visitor.UserAgent,
                UserId = visitor.UserId
            };
        }

        /// <summary>
        /// Convert VisitorInfo to Visitor.
        /// </summary>
        /// <param name="visitorInfo">Visitor info</param>
        /// <returns>Visitor</returns>
        public static Visitor ToVisitor(this VisitorInfo visitorInfo)
        {
            if (visitorInfo is null)
            {
                return null;
            }

            return new Visitor()
            { 
                Id = visitorInfo.Id,
                IpAddress = visitorInfo.IpAddress,
                LastVisitAt = visitorInfo.LastVisitAt,
                UserAgent = visitorInfo.UserAgent,
                UserId = visitorInfo.UserId
            };
        }
    }
}
