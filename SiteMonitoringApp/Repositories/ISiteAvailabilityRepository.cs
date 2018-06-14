using MongoDB.Driver;
using SiteMonitoringApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiteMonitoringApp.Repositories
{
    /// <summary>
    /// Contract for site repository.
    /// </summary>
    public interface ISiteAvailabilityRepository
    {
        /// <summary>
        /// Gets alls sites from DB.
        /// </summary>
        /// <returns>all sites from DB</returns>
        Task<IEnumerable<Site>> GetAllSitesAsync();

        /// <summary>
        /// Gets site by its ID from DB.
        /// </summary>
        /// <param name="id">ID of site</param>
        /// <returns>site</returns>
        Task<Site> GetSiteAsync(Guid id);

        /// <summary>
        /// Creates new site in DB.
        /// </summary>
        /// <param name="item">site with URL that needs to be created in DB</param>
        /// <returns>created site</returns>
        Task<Site> AddSiteAsync(Site item);

        /// <summary>
        /// Removes site by ID from DB.
        /// </summary>
        /// <param name="id">ID of site</param>
        /// <returns>Result of deletion in <see cref="DeleteResult"/> object</returns>
        Task<DeleteResult> RemoveSiteAsync(Guid id);

        /// <summary>
        /// Updates availability of site in DB.
        /// </summary>
        /// <param name="id">ID of site</param>
        /// <param name="isAvailable">True if site is available of False otherwise.</param>
        /// <returns>Result of update in <see cref="UpdateResult"/> object</returns>
        Task<UpdateResult> UpdateSiteAvailabilityAsync(Guid id, bool isAvailable);

        /// <summary>
        /// Updates URL of site in DB.
        /// </summary>
        /// <param name="id">ID of site</param>
        /// <param name="url">new URL of site</param>
        /// <returns>Result of update in <see cref="UpdateResult"/> object</returns>
        Task<UpdateResult> UpdateSiteUrlAsync(Guid id, string url);

        /// <summary>
        /// Gets settings with monitoring time period from DB.
        /// </summary>
        /// <returns>settings monitoring time period in seconds</returns>
        Task<MonitoringSettings> GetMonitoringSettingsAsync();

        /// <summary>
        /// Sets monitoring time period in DB.
        /// If it doesn't exist in DB it will be created.
        /// </summary>
        /// <param name="periodSeconds">new monitoring time period</param>
        /// <returns>True if monitoring time period was successfully set of False otherwise.</returns>
        Task<bool> SetPeriodSecondsAsync(int periodSeconds);

        /// <summary>
        /// Gets monitoring time period from DB.
        /// </summary>
        /// <returns>monitoring time period in seconds</returns>
        Task<int> GetPeriodSecondsAsync();
    }
}
