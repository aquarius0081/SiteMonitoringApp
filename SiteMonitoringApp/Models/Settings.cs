namespace SiteMonitoringApp.Models
{
    /// <summary>
    /// Model for connection settings and database name in MongoDB server.
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// Connection string.
        /// </summary>
        public string ConnectionString;
        
        /// <summary>
        /// Name of database.
        /// </summary>
        public string Database;
    }
}
