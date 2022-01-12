using System;
using System.Collections.Generic;
using System.Linq;
using Kastra.Business.Mappers;
using Kastra.Core.Services.Contracts;
using Kastra.Core.DTO;
using Kastra.Core.Services;
using Kastra.DAL.EntityFramework;
using Kastra.DAL.EntityFramework.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Kastra.Business
{
    public class StatisticsManager : IStatisticsManager
    {
        private const string VISITS_KEY = "visits_list";

        #region Private members

        private readonly KastraDbContext _dbContext;
        private readonly CacheEngine _cacheEngine;

        #endregion

        public StatisticsManager(KastraDbContext dbContext, CacheEngine cacheEngine)
        {
            _dbContext = dbContext;
            _cacheEngine = cacheEngine;
        }

        /// <inheritdoc cref="IStatisticsManager.SaveVisitorAsync(VisitorInfo)"/>
        public async Task<bool> SaveVisitorAsync(VisitorInfo visitorInfo)
        {
            if (visitorInfo is null)
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
                        await _dbContext.SaveChangesAsync();

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
            await _dbContext.SaveChangesAsync();

            return true;
        }

        /// <inheritdoc cref="IStatisticsManager.CountVisitsFromToAsync(DateTime, DateTime)"/>
        public async Task<int> CountVisitsFromToAsync(DateTime fromDate, DateTime toDate)
        {
            return await _dbContext.KastraVisitors
                .Where(v => v.LastVisitAt >= fromDate && v.LastVisitAt <= toDate)
                .CountAsync();
        }

        /// <inheritdoc cref="IStatisticsManager.GetVisitsByUserIdAsync(Guid)"/>
        public async Task<IList<VisitorInfo>> GetVisitsByUserIdAsync(Guid userId)
        {
            return await _dbContext.KastraVisitors
                .Where(v => v.UserId == userId)
                .Select(v => v.ToVisitorInfo())
                .ToListAsync();
        }

        /// <inheritdoc cref="IStatisticsManager.GetVisitsFromDateAsync(DateTime, DateTime)"/>
        public async Task<IList<VisitorInfo>> GetVisitsFromDateAsync(DateTime fromDate, DateTime toDate)
        {
            return await _dbContext.KastraVisitors
                .Where(v => v.LastVisitAt >= fromDate && v.LastVisitAt <= toDate)
                .Select(v => v.ToVisitorInfo())
                .ToListAsync();
        }
    }
}
