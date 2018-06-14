using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SiteMonitoringApp.Data;
using SiteMonitoringApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiteMonitoringApp.Repositories
{
    /// <summary>
    /// Repository for manipulating with sites, their availabilities and monitoring time period.
    /// </summary>
    public class SiteAvailabilityRepository : ISiteAvailabilityRepository
    {
        private readonly SiteAvailabilityContext _context = null;

        /// <summary>
        /// Default monitoring time period in seconds
        /// </summary>
        private const int DefaultPeriodSeconds = 5;

        /// <summary>
        /// Constructs repository with settings for DB.
        /// </summary>
        /// <param name="settings">DB settings</param>
        public SiteAvailabilityRepository(IOptions<Settings> settings)
        {
            _context = new SiteAvailabilityContext(settings);
        }

        /// <see cref="ISiteAvailabilityRepository.GetAllSitesAsync"/>
        public async Task<IEnumerable<Site>> GetAllSitesAsync()
        {
            return await _context.Sites.Find(_ => true).ToListAsync();
        }

        /// <see cref="ISiteAvailabilityRepository.GetSiteAsync(Guid)"/>
        public async Task<Site> GetSiteAsync(Guid id)
        {
            var filter = Builders<Site>.Filter.Eq("Id", id);
            return await _context.Sites
                            .Find(filter)
                            .FirstOrDefaultAsync();
        }

        /// <see cref="ISiteAvailabilityRepository.AddSiteAsync(Site)"/>
        public async Task<Site> AddSiteAsync(Site item)
        {
            await _context.Sites.InsertOneAsync(item);
            return item;
        }

        /// <see cref="ISiteAvailabilityRepository.RemoveSiteAsync(Guid)"/>
        public async Task<DeleteResult> RemoveSiteAsync(Guid id)
        {
            return await _context.Sites.DeleteOneAsync(
                 Builders<Site>.Filter.Eq("Id", id));
        }

        /// <see cref="ISiteAvailabilityRepository.UpdateSiteAvailabilityAsync(Guid,bool)"/>
        public async Task<UpdateResult> UpdateSiteAvailabilityAsync(Guid id, bool isAvailable)
        {
            var filter = Builders<Site>.Filter.Eq(s => s.Id, id);
            var update = Builders<Site>.Update
                            .Set(s => s.IsAvailable, isAvailable);

            return await _context.Sites.UpdateOneAsync(filter, update);
        }

        /// <see cref="ISiteAvailabilityRepository.UpdateSiteUrlAsync(Guid,string)"/>
        public async Task<UpdateResult> UpdateSiteUrlAsync(Guid id, string url)
        {
            var filter = Builders<Site>.Filter.Eq(s => s.Id, id);
            var update = Builders<Site>.Update
                            .Set(s => s.Url, url);

            return await _context.Sites.UpdateOneAsync(filter, update);
        }

        /// <see cref="ISiteAvailabilityRepository.GetMonitoringSettingsAsync"/>
        public async Task<MonitoringSettings> GetMonitoringSettingsAsync()
        {
            return await _context.MonitoringSettings.Find(_ => true).FirstOrDefaultAsync();
        }

        /// <see cref="ISiteAvailabilityRepository.SetPeriodSecondsAsync(int)"/>
        public async Task<bool> SetPeriodSecondsAsync(int periodSeconds)
        {
            if (await GetMonitoringSettingsAsync() == null)
            {
                MonitoringSettings monitoringSettings = new MonitoringSettings { PeriodSeconds = periodSeconds };
                await _context.MonitoringSettings.InsertOneAsync(monitoringSettings);
                return true;
            }
            else
            {
                var filter = Builders<MonitoringSettings>.Filter.Empty;
                var update = Builders<MonitoringSettings>.Update
                                .Set(s => s.PeriodSeconds, periodSeconds);
                var result = await _context.MonitoringSettings.UpdateOneAsync(filter, update);
                if (result.IsAcknowledged)
                {
                    return true;
                }
            }
            return false;
        }

        /// <see cref="ISiteAvailabilityRepository.GetPeriodSecondsAsync"/>
        public async Task<int> GetPeriodSecondsAsync()
        {
            var monitoringSettings = await GetMonitoringSettingsAsync();
            if (monitoringSettings != null)
            {
                return monitoringSettings.PeriodSeconds;
            }
            return DefaultPeriodSeconds;
        }

    }
}
