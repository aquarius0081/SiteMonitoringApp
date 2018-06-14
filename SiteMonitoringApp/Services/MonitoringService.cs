using SiteMonitoringApp.Models;
using SiteMonitoringApp.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SiteMonitoringApp.Services
{
    /// <summary>
    /// Background service for monitoring of sites.
    /// </summary>
    public class MonitoringService : HostedService
    {
        private readonly ISiteAvailabilityProvider _siteAvailabilityProvider;
        private readonly ISiteAvailabilityRepository _siteAvailabilityRepository;

        /// <summary>
        /// Constructs services with provider and repository.
        /// </summary>
        /// <param name="siteAvailabilityProvider">provider</param>
        /// <param name="siteAvailabilityRepository">repository</param>
        public MonitoringService(ISiteAvailabilityProvider siteAvailabilityProvider, ISiteAvailabilityRepository siteAvailabilityRepository)
        {
            _siteAvailabilityProvider = siteAvailabilityProvider;
            _siteAvailabilityRepository = siteAvailabilityRepository;
        }

        /// <summary>
        /// Executes monitoring for all sites in DB and updates their availability status.
        /// </summary>
        /// <param name="cancellationToken">cancellation token</param>
        /// <returns><see cref="Task"/></returns>
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                IEnumerable<Site> sites = await _siteAvailabilityRepository.GetAllSitesAsync();
                foreach (var site in sites)
                {
                    _siteAvailabilityProvider.OpenSite(site.Id, site.Url, cancellationToken);
                }
                await Task.Delay(TimeSpan.FromSeconds(await _siteAvailabilityRepository.GetPeriodSecondsAsync()), cancellationToken);
            }
        }
    }
}