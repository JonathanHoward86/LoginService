using NLog.Web;
using System.Net;

namespace LoginService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Configure NLog using the new recommended approach
            NLog.LogManager.Setup().LoadConfigurationFromAppSettings();
            var logger = NLog.LogManager.GetCurrentClassLogger();

            try
            {
                logger.Info("Application starting up.");
                CreateHostBuilder(args, logger).Build().Run();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Application stopped because of exception.");
                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args, NLog.Logger logger) =>
                    Host.CreateDefaultBuilder(args)
                        .ConfigureLogging(logging =>
                        {
                            logging.ClearProviders();
                            logging.SetMinimumLevel(LogLevel.Trace);
                        })
                        .UseNLog()
                        .ConfigureWebHostDefaults(webBuilder =>
                        {
                            webBuilder.UseStartup<Startup>()
                            .UseKestrel(options =>
                            {
                                options.Listen(IPAddress.Loopback, 5002);  // HTTP
                                try
                                {
                                    options.Listen(IPAddress.Loopback, 5001, listenOptions =>
                                    {
                                        listenOptions.UseHttps("C:\\Coding\\sslcert.pfx", "1Test@");
                                    });  // HTTPS
                                }
                                catch (Exception ex)
                                {
                                    logger.Error(ex, "Failed to start HTTPS.");
                                    throw;
                                }
                            });
                        });
    }
}