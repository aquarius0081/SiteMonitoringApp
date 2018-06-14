using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.ComponentModel.DataAnnotations;

namespace SiteMonitoringApp.Models
{
    /// <summary>
    /// Model for sites.
    /// </summary>
    public class Site
    {
        /// <summary>
        /// ID of site.
        /// </summary>
        [BsonId(IdGenerator = typeof(GuidGenerator))]
        public Guid Id { get; set; }

        /// <summary>
        /// URL of site.
        /// </summary>
        [Required]
        [MinLength(1)]
        public string Url { get; set; }

        /// <summary>
        /// True if site is available and false otherwise.
        /// </summary>
        public bool IsAvailable { get; set; }
    }
}
