using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.WindowsServices;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using System;
using System.IO;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace FormsHandler
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string baseDir = Directory.GetCurrentDirectory();
            if (WindowsServiceHelpers.IsWindowsService())
            {
                baseDir = AppDomain.CurrentDomain.BaseDirectory;
            }

            try
            {
                CreateHostBuilder(args, baseDir).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, $"API start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args, string baseDir) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.SetBasePath(baseDir)
                      .AddJsonFile($"appsettings.json")
                      .Build();
            })
            .ConfigureServices((context, services) =>
            {
                services.Configure<KestrelServerOptions>(context.Configuration.GetSection("Kestrel"));
                var loggerConfig = new LoggerConfiguration().ReadFrom.Configuration(context.Configuration);
                Log.Logger = loggerConfig.CreateLogger();
                services.AddSingleton(Log.Logger);
                services.AddSingleton<ILoggerFactory>(x => new SerilogLoggerFactory(null, false));

                var ver = typeof(Startup).Assembly.GetName().Version.ToString();
                Log.Information(WindowsServiceHelpers.IsWindowsService()
                    ? $"Starting Windows Service ( ver.{ver})"
                    : $"Starting Web host Service ( ver.{ver})");
            })
            .ConfigureLogging((context, logging) =>
            {
                logging.AddSerilog(logging.Services
                    .BuildServiceProvider()
                    .GetRequiredService<Serilog.ILogger>(),
                    dispose: true);
            })
            .UseSerilog()
            .UseWindowsService()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}
