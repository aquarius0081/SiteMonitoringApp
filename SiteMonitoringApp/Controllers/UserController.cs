using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SiteMonitoringApp.Models;

namespace SiteMonitoringApp.Controllers
{
    /// <summary>
    /// API controller for user authentication.
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AdminUserSettings _adminUserSettings;

        /// <summary>
        /// Constructs controller with Administrator username and password.
        /// </summary>
        /// <param name="adminUserSettings">Admin username and password</param>
        public UserController(IOptions<AdminUserSettings> adminUserSettings)
        {
            _adminUserSettings = adminUserSettings.Value;
        }

        /// <summary>
        /// Performs user authentication.
        /// Use POST api/user.
        /// </summary>
        /// <param name="user">View model for user credentials</param>
        /// <returns>
        /// Success response with true if user username and password matches Administrator credentials
        /// or fail response with false otherwise.
        /// </returns>
        [HttpPost]
        public IActionResult Authenticate([FromBody] UserViewModel user)
        {
            if (user.Username == _adminUserSettings.UserName && user.Password == _adminUserSettings.Password)
            {
                return Ok(true);
            }
            return Ok(false);
        }
    }
}