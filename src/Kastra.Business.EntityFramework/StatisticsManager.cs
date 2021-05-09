using System;
using System.Collections.Generic;
using System.Linq;
using Kastra.Business.Mappers;
using Kastra.Core.Services.Contracts;
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

        private readonly KastraDbContext _dbContext = null;
        private readonly CacheEngine _cacheEngine = null;

        #endregion

        public StatisticsManager(KastraDbContext dbContext, CacheEngine cacheEngine)
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

            bool isLoggedIn = visitorInfo.UserId.HasValue;
            string ipAddress = visitorInfo.IpAddress;

            if (_cacheEngine.GetCacheObject(VISITS_KEY, out Dictionary<string, bool> recentVisitors))
            {
                // If visitor exists in cache
                if (recentVisitors.ContainsKey(ipAddress))
                {
                    if(isLoggedIn && !recentVisitors[ipAddress])
                    {
                        // Update in cache
                        recentVisitors[ipAddress] = isLoggedIn;

                        // Update existing visit
                        DateTime startDate = DateTime.UtcNow.Subtract(_cacheEngine.CacheOptions?.SlidingExpiration ?? new TimeSpan());
                        Visitor visit = _dbContext.KastraVisitors
                            .Where(v => v.LastVisitAt >= startDate && v.LastVisitAt <= DateTime.UtcNow)
                            .SingleOrDefault(v => v.IpAddress == ipAddress);

                        if (visit is null)
                        {
                            return false;
                        }

                        visit.UserId = visit.UserId;

                        _dbContext.KastraVisitors.Update(visit);
                        _dbContext.SaveChanges();

                        return true;
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
                _cacheEngine.SetCacheObject(VISITS_KEY, recentVisitors);
            }

            // Save visit in database
            Visitor visitor = visitorInfo.ToVisitor();

            _dbContext.KastraVisitors.Add(visitor);
            _dbContext.SaveChanges();

            return true;
        }

        public int CountVisitsFromTo(DateTime fromDate, DateTime toDate)
        {
            return _dbContext.KastraVisitors.Where(v => v.LastVisitAt >= fromDate && v.LastVisitAt <= toDate).Count();
        }

        public IList<VisitorInfo> GetVisitsByUserId(Guid userId)
        {
            return _dbContext.KastraVisitors
                .Where(v => v.UserId == userId)
                .Select(v => v.ToVisitorInfo())
                .ToList();
        }

        public IList<VisitorInfo> GetVisitsFromDate(DateTime fromDate, DateTime toDate)
        {
            return _dbContext.KastraVisitors
                .Where(v => v.LastVisitAt >= fromDate && v.LastVisitAt <= toDate)
                .Select(v => v.ToVisitorInfo())
                .ToList();
        }
    }
}
