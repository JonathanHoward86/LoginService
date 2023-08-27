using NLog.Web;
using System.Net;

namespace LoginService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Initialize NLog configurations from app settings
            NLog.LogManager.Setup().LoadConfigurationFromAppSettings();
            var logger = NLog.LogManager.GetCurrentClassLogger();

            try
            {
                // Log application startup
                logger.Info("Application starting up.");

                // Create and run the host
                CreateHostBuilder(args, logger).Build().Run();
            }
            catch (Exception ex)
            {
                // Log any exceptions that stop the application
                logger.Error(ex, "Application stopped because of exception.");
                throw;
            }
            finally
            {
                // Shutdown NLog when application ends
                NLog.LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args, NLog.Logger logger) =>
                    Host.CreateDefaultBuilder(args)
                        .ConfigureLogging(logging =>
                        {
                            // Clear existing logging providers and set log level
                            logging.ClearProviders();
                            logging.SetMinimumLevel(LogLevel.Trace);
                        })
                        .UseNLog()  // Use NLog for logging
                        .ConfigureWebHostDefaults(webBuilder =>
                        {
                            // Use Startup class and configure Kestrel server
                            webBuilder.UseStartup<Startup>()
                            .UseKestrel(options =>
                            {
                                // Listen on HTTP port 5002
                                options.Listen(IPAddress.Loopback, 5002);

                                // Listen on HTTPS port 5001
                                try
                                {
                                    options.Listen(IPAddress.Loopback, 5001, listenOptions =>
                                    {
                                        listenOptions.UseHttps("C:\\Coding\\sslcert.pfx", "1Test@");
                                    });
                                }
                                catch (Exception ex)
                                {
                                    // Log failure to start HTTPS
                                    logger.Error(ex, "Failed to start HTTPS.");
                                    throw;
                                }
                            });
                        });
    }
}