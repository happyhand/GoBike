using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Web;
using System;

namespace GoBike.MGT.APP
{
    /// <summary>
    /// Program
    /// </summary>
    public class Program
    {
        /// <summary>
        /// CreateWebHostBuilder
        /// </summary>
        /// <param name="args">args</param>
        /// <returns>IWebHostBuilder</returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                      .ConfigureAppConfiguration((webHostBuilder, configurationBinder) =>
                      {
                          configurationBinder.AddJsonFile("appsettings.json", optional: true);
                      })
                      .UseStartup<Startup>()
                      .UseNLog();
        }

        /// <summary>
        /// Main
        /// </summary>
        /// <param name="args">args</param>
        public static void Main(string[] args)
        {
            // NLog: setup the logger first to catch all errors
            Logger logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

            try
            {
                logger.Debug("init main");
                CreateWebHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                //NLog: catch setup errors
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
        }

        //WebHost.CreateDefaultBuilder(args)
        //			.ConfigureAppConfiguration((hostContext, config) =>
        //			{
        //				var env = hostContext.HostingEnvironment;
        //				config
        //				.AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: true)
        //				.AddJsonFile(path: $"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
        //			})
        //			.UseStartup<Startup>()
        //			.ConfigureLogging(logging =>
        //			{
        //				logging.ClearProviders();
        //				logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
        //			})
        //			.UseNLog();  // NLog: setup NLog for Dependency injection
    }
}