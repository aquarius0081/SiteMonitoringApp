using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SiteMonitoringApp.Models;
using SiteMonitoringApp.Repositories;

namespace SiteMonitoringApp.Controllers
{
    /// <summary>
    /// API controller for manipulating with sites and their availabilities in DB through MongoDB.Driver.
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class SiteAvailabilityController : Controller
    {
        private readonly ISiteAvailabilityRepository _siteAvailabilityRepository;

        /// <summary>
        /// Constructs controller with repository.
        /// </summary>
        /// <param name="siteAvailabilityRepository">Repository</param>
        public SiteAvailabilityController(ISiteAvailabilityRepository siteAvailabilityRepository)
        {
            _siteAvailabilityRepository = siteAvailabilityRepository;
        }

        /// <summary>
        /// Gets all sites from DB.
        /// Use GET api/siteavailability.
        /// </summary>
        /// <returns>All sites in DB</returns>
        [HttpGet]
        public Task<IEnumerable<Site>> Get()
        {
            return GetSiteInternal();
        }

        private async Task<IEnumerable<Site>> GetSiteInternal()
        {
            return await _siteAvailabilityRepository.GetAllSitesAsync();
        }

        /// <summary>
        /// Gets site from DB by ID.
        /// Use GET api/siteavailability/{id}.
        /// </summary>
        /// <param name="id">ID of site</param>
        /// <returns>site</returns>
        [HttpGet("{id}")]
        public Task<Site> GetById(string id)
        {
            return GetSiteByIdInternal(new Guid(id));
        }

        private async Task<Site> GetSiteByIdInternal(Guid id)
        {
            return await _siteAvailabilityRepository.GetSiteAsync(id) ?? new Site();
        }

        /// <summary>
        /// Creates new site in DB.
        /// Use POST api/siteavailability.
        /// </summary>
        /// <param name="site">model with values for creation</param>
        /// <returns>
        /// Success response together with created site if it was successfully created in DB.
        /// Fail response in case if model is not valid
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] Site site)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var insertedSite = await _siteAvailabilityRepository.AddSiteAsync(site);

            return Ok(insertedSite);
        }

        /// <summary>
        /// Updates site URL in DB.
        /// Use PUT api/siteavailability/{id}.
        /// </summary>
        /// <param name="id">ID of site that needs to be updated</param>
        /// <param name="site">model with new URL that needs to be set</param>
        /// <returns>
        /// Success response if site URL was updated successfully.
        /// Fail response if model is not valid or if site was not updated in DB
        /// </returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(string id, [FromBody] Site site)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _siteAvailabilityRepository.UpdateSiteUrlAsync(new Guid(id), site.Url);

            if (!result.IsAcknowledged)
            {
                return UnprocessableEntity();
            }
            return Ok();
        }

        /// <summary>
        /// Deletes site from DB.
        /// Use DELETE api/siteavailability/{id}.
        /// </summary>
        /// <param name="id">ID of site that needs to be deleted</param>
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            _siteAvailabilityRepository.RemoveSiteAsync(new Guid(id));
        }

        /// <summary>
        /// Sets monitoring time period in seconds in DB.
        /// Use POST api/siteavailability/period.
        /// </summary>
        /// <param name="monitoringSettings">model with new time period that needs to be set</param>
        /// <returns>
        /// Success response if monitoring time period was updated successfully.
        /// Fail response if model is not valid or if time period was not updated in DB
        /// </returns>
        [HttpPost("period")]
        public async Task<IActionResult> PostPeriodSecondsAsync([FromBody] MonitoringSettings monitoringSettings)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _siteAvailabilityRepository.SetPeriodSecondsAsync(monitoringSettings.PeriodSeconds);
            if (!result)
            {
                return UnprocessableEntity();
            }
            return Ok();
        }

        /// <summary>
        /// Gets monitoring time period in seconds from DB.
        /// Use GET api/siteavailability/period.
        /// </summary>
        /// <returns>monitoring time period in seconds</returns>
        [HttpGet("period")]
        public async Task<int> GetPeriodSecondsAsync()
        {
            return await _siteAvailabilityRepository.GetPeriodSecondsAsync();
        }
    }
}