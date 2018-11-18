using System;
using System.Collections.Generic;
using System.Linq;
using Kastra.Business.Mappers;
using Kastra.Core.Business;
using Kastra.Core.DTO;
using Kastra.Core.Services;
using Kastra.DAL.EntityFramework;
using Kastra.DAL.EntityFramework.Models;

namespace Kastra.Business
{
    public class StatisticsManager : IStatisticsManager
    {
        private const string VISITS_KEY = "visits_list";

        #region Private members

        private readonly KastraContext _dbContext = null;
        private readonly CacheEngine _cacheEngine = null;

        #endregion

        public StatisticsManager(KastraContext dbContext, CacheEngine cacheEngine)
        {
            _dbContext = dbContext;
            _cacheEngine = cacheEngine;
        }

        public bool SaveVisitor(VisitorInfo visitorInfo)
        {
            Dictionary<string, DateTime> recentVisitors = null;

            if (_cacheEngine.GetCacheObject<Dictionary<string, DateTime>>(VISITS_KEY, out recentVisitors))
            {
                if (recentVisitors.ContainsKey(visitorInfo.IpAddress))
                {
                    return false;
                }
                else 
                {
                    recentVisitors.Add(visitorInfo.IpAddress, DateTime.UtcNow);
                }
            }
            else
            {
                recentVisitors = new Dictionary<string, DateTime>();
                recentVisitors.Add(visitorInfo.IpAddress, DateTime.UtcNow);
                _cacheEngine.SetCacheObject<Dictionary<string, DateTime>>(VISITS_KEY, recentVisitors);
            }

            // Save visit in database
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
