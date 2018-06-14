using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SiteMonitoringApp.Models;
using SiteMonitoringApp.Repositories;
using SiteMonitoringApp.Services;

namespace SiteMonitoringApp
{
    /// <summary>
    /// Configuration of application with initial settings.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Constructs <see cref="Startup"/> with specified configuration.
        /// </summary>
        /// <param name="configuration">Configuration set for application.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Configuration set for application.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Used this method to add services to the container.
        /// Added the following services DB connection settings, MVC services, CORS and Admin user settings.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowAnyOrigin()
                .AllowCredentials();
            }));

            services.AddMvc();

            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            services.AddSingleton<SiteAvailabilityProvider>();
            services.AddSingleton<IHostedService, MonitoringService>();

            services.Configure<Settings>(options =>
            {
                options.ConnectionString = Configuration.GetSection("MongoConnection:ConnectionString").Value;
                options.Database = Configuration.GetSection("MongoConnection:Database").Value;
            });

            services.Configure<AdminUserSettings>(admin =>
            {
                admin.UserName = Configuration.GetSection("AdminUser:UserName").Value;
                admin.Password = Configuration.GetSection("AdminUser:Password").Value;
            });

            services.AddTransient<ISiteAvailabilityRepository, SiteAvailabilityRepository>();
            services.AddTransient<ISiteAvailabilityProvider, SiteAvailabilityProvider>();
        }

        /// <summary>
        /// Used this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"><see cref="IApplicationBuilder"/></param>
        /// <param name="env"><see cref="IHostingEnvironment"/></param>
        /// <param name="loggerFactory"><see cref="ILoggerFactory"/></param>
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            var loggingConfiguration = Configuration.GetSection("Logging");
            loggerFactory.AddConsole(loggingConfiguration);
            loggerFactory.AddDebug();
            loggerFactory.AddFile(loggingConfiguration["LogFilePath"]);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseCors("CorsPolicy");

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }
    }
}
