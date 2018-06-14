using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SiteMonitoringApp
{
    /// <summary>
    /// Entry point for web host.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main method of program. Builds a web host for application
        /// and runs web application.
        /// </summary>
        /// <param name="args">Array of arguments</param>
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        /// <summary>
        /// Builds web host.
        /// </summary>
        /// <param name="args">Array of arguments</param>
        /// <returns>Web host for application</returns>
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
