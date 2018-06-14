using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Driver;
using NSubstitute;
using SiteMonitoringApp.Controllers;
using SiteMonitoringApp.Models;
using SiteMonitoringApp.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiteMonitoringApp.Tests
{
    /// <summary>
    /// Unit tests for <see cref="SiteAvailabilityController"/> class.
    /// </summary>
    [TestClass]
    public class SiteAvailabilityControllerTest
    {
        /// <summary>
        /// Tests that <see cref="SiteAvailabilityController.Get"/> executed sucessfully
        /// and returned specified list of sites.
        /// </summary>
        [TestMethod]
        public async Task TestGetSuccess()
        {
            var repository = Substitute.For<ISiteAvailabilityRepository>();
            var controller = new SiteAvailabilityController(repository);
            var sites = new List<Site>();
            sites.Add(new Site { Id = Guid.NewGuid(), Url = "url1", IsAvailable = false });
            sites.Add(new Site { Id = Guid.NewGuid(), Url = "url2", IsAvailable = true });
            repository.GetAllSitesAsync().Returns(Task.FromResult<IEnumerable<Site>>(sites));
            var result = await controller.Get();
            Assert.AreEqual(sites, result);
        }

        /// <summary>
        /// Tests that <see cref="SiteAvailabilityController.GetById(string)"/> executed successfully
        /// and returned requested site.
        /// </summary>
        [TestMethod]
        public async Task TestGetByIdSuccess()
        {
            var repository = Substitute.For<ISiteAvailabilityRepository>();
            var controller = new SiteAvailabilityController(repository);
            var id = Guid.NewGuid();
            var site = new Site { Id = id, Url = "url1", IsAvailable = false };
            repository.GetSiteAsync(id).Returns(Task.FromResult<Site>(site));
            var result = await controller.GetById(id.ToString());
            Assert.AreEqual(site, result);
        }

        /// <summary>
        /// Tests that <see cref="SiteAvailabilityController.PostAsync(Site)"/> executed
        /// successfully and returned success response with created site.
        /// </summary>
        [TestMethod]
        public async Task TestPostAsyncSuccess()
        {
            var repository = Substitute.For<ISiteAvailabilityRepository>();
            var controller = new SiteAvailabilityController(repository);
            var site = new Site { Id = Guid.NewGuid(), Url = "url1", IsAvailable = false };
            repository.AddSiteAsync(site).Returns(Task.FromResult<Site>(site));
            var result = await controller.PostAsync(site);
            Assert.IsTrue(result is OkObjectResult);
            var successResult = (OkObjectResult)result;
            Assert.AreEqual(site, successResult.Value);
        }

        /// <summary>
        /// Tests that execution of <see cref="SiteAvailabilityController.PostAsync(Site)"/> fails
        /// in case of model validation fails.
        /// </summary>
        [TestMethod]
        public async Task TestPostAsyncFail()
        {
            var repository = Substitute.For<ISiteAvailabilityRepository>();
            var controller = new SiteAvailabilityController(repository);
            var site = new Site { Id = Guid.NewGuid(), Url = "", IsAvailable = false };
            repository.AddSiteAsync(site).Returns(Task.FromResult<Site>(site));
            controller.ValidateViewModel<Site, SiteAvailabilityController>(site);
            var result = await controller.PostAsync(site);
            Assert.IsTrue(result is BadRequestObjectResult);
            var failResult = (BadRequestObjectResult)result;
            Assert.AreEqual(StatusCodes.Status400BadRequest, failResult.StatusCode);
            Assert.AreEqual(false, controller.ModelState.IsValid);
        }

        /// <summary>
        /// Tests that <see cref="SiteAvailabilityController.PutAsync(string, Site)"/> executed
        /// successfully and returned success response.
        /// </summary>
        [TestMethod]
        public async Task TestPutAsyncSuccess()
        {
            var repository = Substitute.For<ISiteAvailabilityRepository>();
            var controller = new SiteAvailabilityController(repository);
            var id = Guid.NewGuid();
            var site = new Site { Id = id, Url = "url1", IsAvailable = false };
            repository.UpdateSiteUrlAsync(id, site.Url).Returns(Task.FromResult<UpdateResult>(new UpdateResult.Acknowledged(1,1, BsonValue.Create(id))));
            var result = await controller.PutAsync(id.ToString(), site);
            Assert.IsTrue(result is OkResult);
        }

        /// <summary>
        /// Tests that <see cref="SiteAvailabilityController.PostPeriodSecondsAsync(MonitoringSettings)"/>
        /// executed successfully and returned success response.
        /// </summary>
        [TestMethod]
        public async Task TestPostPeriodSecondsAsyncSuccess()
        {
            var repository = Substitute.For<ISiteAvailabilityRepository>();
            var controller = new SiteAvailabilityController(repository);
            var monitoringSettings = new MonitoringSettings { PeriodSeconds = 10 };
            repository.SetPeriodSecondsAsync(monitoringSettings.PeriodSeconds).Returns(Task.FromResult<bool>(true));
            var result = await controller.PostPeriodSecondsAsync(monitoringSettings);
            Assert.IsTrue(result is OkResult);
        }

        /// <summary>
        /// Tests that <see cref="SiteAvailabilityController.GetPeriodSecondsAsync"/> executed successfully
        /// and returned actual monitoring time period.
        /// </summary>
        [TestMethod]
        public async Task TestGetPeriodSecondsAsyncSuccess()
        {
            var repository = Substitute.For<ISiteAvailabilityRepository>();
            var controller = new SiteAvailabilityController(repository);
            repository.GetPeriodSecondsAsync().Returns(Task.FromResult(5));
            var result = await controller.GetPeriodSecondsAsync();
            Assert.AreEqual(5, result);
        }
    }
}
