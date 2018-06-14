using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using SiteMonitoringApp.Controllers;
using SiteMonitoringApp.Models;

namespace SiteMonitoringApp.Tests
{
    /// <summary>
    /// Unit tests for <see cref="UserController"/> class.
    /// </summary>
    [TestClass]
    public class UserControllerTest
    {
        /// <summary>
        /// Tests that authentication succeed in case if provided username and password matches
        /// Administrator credentials.
        /// </summary>
        [TestMethod]
        public void TestAuthenticateSuccess()
        {
            var settings = Substitute.For<IOptions<AdminUserSettings>>();
            settings.Value.UserName = "user";
            settings.Value.Password = "password";
            var controller = new UserController(settings);
            var model = new UserViewModel { Username = "user", Password = "password" };
            IActionResult result = controller.Authenticate(model);
            Assert.IsTrue(result is OkObjectResult);
            var successResult = (OkObjectResult)result;
            Assert.IsTrue((bool)successResult.Value);
        }

        /// <summary>
        /// Tests that authentication fails in case if provided username and password doesn't
        /// match Administrator credentials.
        /// </summary>
        [TestMethod]
        public void TestAuthenticateFail()
        {
            var settings = Substitute.For<IOptions<AdminUserSettings>>();
            settings.Value.UserName = "user";
            settings.Value.Password = "password";
            var controller = new UserController(settings);
            var model = new UserViewModel { Username = "otherUser", Password = "otherPassword" };
            IActionResult result = controller.Authenticate(model);
            Assert.IsTrue(result is OkObjectResult);
            var failResult = (OkObjectResult)result;
            Assert.IsFalse((bool)failResult.Value);
        }
    }
}
