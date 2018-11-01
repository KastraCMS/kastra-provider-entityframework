using System;
using System.Collections.Generic;
using System.Linq;
using Kastra.Business.Mappers;
using Kastra.Core.Business;
using Kastra.Core.DTO;
using Kastra.DAL.EntityFramework;
using Kastra.DAL.EntityFramework.Models;

namespace Kastra.Business
{
    public class StatisticsManager : IStatisticsManager
    {
        private readonly KastraContext _dbContext = null;

        public StatisticsManager(KastraContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool SaveVisitor(VisitorInfo visitorInfo)
        {
            KastraVisitors visitor = visitorInfo.ToKastraVisitor();

            _dbContext.KastraVisitors.Add(visitor);
            _dbContext.SaveChanges();

            return true;
        }

        public int CountVisitsFromTo(DateTime fromDate, DateTime toDate)
        {
            return _dbContext.KastraVisitors.Where(v => v.LastVisitAt >= fromDate && v.LastVisitAt <= toDate).Count();
        }

        public IList<VisitorInfo> GetVisitsByUserId(string userId)
        {
            return _dbContext.KastraVisitors.Where(v => v.UserId == userId)
                             .Select(v => v.ToVisitorInfo()).ToList();
        }

        public IList<VisitorInfo> GetVisitsFromDate(DateTime fromDate, DateTime toDate)
        {
            return _dbContext.KastraVisitors.Where(v => v.LastVisitAt >= fromDate && v.LastVisitAt <= toDate)
                             .Select(v => v.ToVisitorInfo()).ToList();
        }
    }
}
