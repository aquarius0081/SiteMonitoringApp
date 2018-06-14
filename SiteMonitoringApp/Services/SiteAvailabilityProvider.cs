using Microsoft.Extensions.Logging;
using SiteMonitoringApp.Repositories;
using System;
using System.Net.Http;
using System.Threading;

namespace SiteMonitoringApp
{
    /// <summary>
    /// Implementation for provider for site availability.
    /// </summary>
    public class SiteAvailabilityProvider : ISiteAvailabilityProvider
    {
        private readonly ILogger<SiteAvailabilityProvider> _logger;
        private readonly HttpClient _httpClient;
        private readonly ISiteAvailabilityRepository _siteAvailabilityRepository;

        /// <summary>
        /// Constructs provider with site repository and logger
        /// </summary>
        /// <param name="siteAvailabilityRepository">site repository</param>
        /// <param name="logger">instance of logger</param>
        public SiteAvailabilityProvider(ISiteAvailabilityRepository siteAvailabilityRepository, ILogger<SiteAvailabilityProvider> logger)
        {
            _httpClient = new HttpClient();
            _siteAvailabilityRepository = siteAvailabilityRepository;
            _logger = logger;
        }

        /// <see cref="ISiteAvailabilityProvider.OpenSite(Guid, string, CancellationToken)"/>
        public async void OpenSite(Guid id, string siteUrl, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _httpClient.GetAsync(siteUrl, cancellationToken);
                _siteAvailabilityRepository.UpdateSiteAvailabilityAsync(id, response.IsSuccessStatusCode);
            }
            catch (Exception ex)
            {
                _logger.LogError($"<< OpenSite : exception during opening site: {ex}");
                _siteAvailabilityRepository.UpdateSiteAvailabilityAsync(id, false);
            }
        }
    }
}
