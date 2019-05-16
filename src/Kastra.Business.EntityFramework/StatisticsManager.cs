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
            if (visitorInfo == null)
            {
                throw new ArgumentNullException(nameof(visitorInfo));
            }

            bool isLoggedIn = !String.IsNullOrEmpty(visitorInfo.UserId);
            string ipAddress = visitorInfo.IpAddress;
            Dictionary<string, bool> recentVisitors = null;

            if (_cacheEngine.GetCacheObject<Dictionary<string, bool>>(VISITS_KEY, out recentVisitors))
            {
                // If visitor exists in cache
                if (recentVisitors.ContainsKey(ipAddress))
                {
                    if(isLoggedIn && !recentVisitors[ipAddress])
                    {
                        // Update in cache
                        recentVisitors[ipAddress] = isLoggedIn;

                        // Update existing visit
                        DateTime startDate = DateTime.UtcNow.Subtract(_cacheEngine.CacheOptions.SlidingExpiration ?? new TimeSpan());
                        KastraVisitors visit = _dbContext.KastraVisitors.Where(v => v.LastVisitAt >= startDate && v.LastVisitAt <= DateTime.UtcNow)
                                                .SingleOrDefault(v => v.IpAddress == ipAddress);

                        if (visit == null)
                        {
                            return false;
                        }

                        visit.UserId = visit.UserId;
                        _dbContext.KastraVisitors.Update(visit);
                        _dbContext.SaveChanges();
                    }

                    return false;
                }
                else 
                {
                    recentVisitors.Add(visitorInfo.IpAddress, isLoggedIn);
                }
            }
            else
            {
                recentVisitors = new Dictionary<string, bool>();
                recentVisitors.Add(visitorInfo.IpAddress, isLoggedIn);
                _cacheEngine.SetCacheObject<Dictionary<string, bool>>(VISITS_KEY, recentVisitors);
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
