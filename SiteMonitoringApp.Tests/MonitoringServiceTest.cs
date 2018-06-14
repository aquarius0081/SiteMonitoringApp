using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using SiteMonitoringApp.Models;
using SiteMonitoringApp.Repositories;
using SiteMonitoringApp.Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SiteMonitoringApp.Tests
{
    /// <summary>
    /// Unit tests for <see cref="MonitoringService"/> class.
    /// </summary>
    [TestClass]
    public class MonitoringServiceTest
    {
        /// <summary>
        /// Tests that execution of monitoring service starts and interruted after specified period of time.
        /// </summary>
        /// <returns><see cref="Task"/></returns>
        [TestMethod]
        public async Task TestMonitoringServiceSuccessAsync()
        {
            var provider = Substitute.For<ISiteAvailabilityProvider>();
            var repository = Substitute.For<ISiteAvailabilityRepository>();
            var service = new MonitoringService(provider, repository);
            var cts = new CancellationTokenSource();
            cts.CancelAfter(3000);
            var sites = new List<Site>();
            sites.Add(new Site { Id = Guid.NewGuid(), Url = "url1", IsAvailable = false });
            sites.Add(new Site { Id = Guid.NewGuid(), Url = "url2", IsAvailable = true });
            repository.GetAllSitesAsync().Returns(Task.FromResult<IEnumerable<Site>>(sites));
            try
            {
                await service.StartAsync(cts.Token);
                Assert.Fail(); //This line should be unreachable
            }
            catch (TaskCanceledException e)
            {
                Assert.AreEqual("A task was canceled.", e.Message);
            }
        }

    }
}
