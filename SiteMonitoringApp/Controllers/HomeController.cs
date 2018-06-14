using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace SiteMonitoringApp.Controllers
{
    /// <summary>
    /// Home controller for ASP.NET Core Site Monitoring application.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Main method of controller.
        /// </summary>
        /// <returns>Main view</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Handling server errors.
        /// </summary>
        /// <returns>View representing server error in application</returns>
        public IActionResult Error()
        {
            ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return View();
        }
    }
}
