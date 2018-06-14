using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SiteMonitoringApp.Models;

namespace SiteMonitoringApp.Data
{
    /// <summary>
    /// MongoDB context.
    /// </summary>
    public class SiteAvailabilityContext
    {
        private readonly IMongoDatabase _database = null;

        /// <summary>
        /// Constructs context with connection settings and database name.
        /// </summary>
        /// <param name="settings">connection settings and database name</param>
        public SiteAvailabilityContext(IOptions<Settings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
                _database = client.GetDatabase(settings.Value.Database);
        }

        /// <summary>
        /// Property for collection with sites.
        /// </summary>
        public IMongoCollection<Site> Sites
        {
            get
            {
                return _database.GetCollection<Site>("Site");
            }
        }

        /// <summary>
        /// Property for collection with monitoring time period.
        /// </summary>
        public IMongoCollection<MonitoringSettings> MonitoringSettings
        {
            get
            {
                return _database.GetCollection<MonitoringSettings>("MonitoringSettings");
            }
        }
    }
}
