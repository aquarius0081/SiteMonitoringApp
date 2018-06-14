using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace SiteMonitoringApp.Models
{
    /// <summary>
    /// Model for monitoring time period.
    /// </summary>
    public class MonitoringSettings
    {
        /// <summary>
        /// ID of monitoring time period.
        /// </summary>
        [BsonId]
        public object Id { get; set; }

        /// <summary>
        /// Monitoring time period in seconds.
        /// </summary>
        [Range(1, 2147483648)]
        public int PeriodSeconds { get; set; }
    }
}
