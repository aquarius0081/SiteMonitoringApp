using System;
using System.Threading;

namespace SiteMonitoringApp
{
    /// <summary>
    /// Contract for provider for site availability.
    /// </summary>
    public interface ISiteAvailabilityProvider
    {
        /// <summary>
        /// Opens URL of site and updates its availability in DB.
        /// </summary>
        /// <param name="id">ID of site in DB</param>
        /// <param name="siteUrl">URL of site</param>
        /// <param name="cancellationToken">cancellation token</param>
        void OpenSite(Guid id, string siteUrl, CancellationToken cancellationToken);
    }
}