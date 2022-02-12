using Infrastructure.Helpers;
using Infrastructure.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Model.Entities;
using Newtonsoft.Json;
using Npgsql;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dal.General
{
    public class TimedHostedService : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public TimedHostedService(IConfiguration config, ILogger logger)
        {
            _configuration = config;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.Information("TimedHostedService - StartAsync Start!");

            try
            {
                var configSection = _configuration.GetSection("AppConfig");
                var connectionString = _configuration.GetSection("ConnectionStrings").GetValue<string>("4CASTDatabase");
                var appConfig = configSection?.Get<AppConfig>();
                string formsApiUrl = appConfig.Endpoints.GetValueOrDefault("FormsApi");

                var DailyTime = appConfig.CheckActivitiesTime;

                var timer = new System.Threading.Timer((e) =>
                {
                    StartActivity(connectionString, formsApiUrl, appConfig.CheckActivitiesTime);
                }, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));

            }
            catch (Exception ex)
            {
                _logger.Error(ex, "TimedHostedService - StartAsync Failed!");
            }

            return Task.CompletedTask;
        }

        private void StartActivity(string connectionString, string formsApiUrl, string importTime)
        {
            try
            {
                var currentDate = DateTime.Now;
                var timeParts = importTime.Split(new char[1] { ':' });
                var importDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day,
                           int.Parse(timeParts[0]), int.Parse(timeParts[1]), 0);

                if (currentDate < importDate || currentDate > importDate.AddMinutes(1))
                    return;

                _logger.Information("TimedHostedService - StartActivity Begin!");

                //loop all activities and check if date on
                using (var DbContext = new Context())
                {
                    var activities = DbContext.Activity.Where(a => Util.ConvertStringToDate(a.StartDate) > DateTime.Now).ToList();
                }

                //create activity object
                string activity = null;

                using (var client = new HttpClient())
                {
                    string url = formsApiUrl + "api/FormsApi/handler/StartActivity";
                    HttpContent c = new StringContent(JsonConvert.SerializeObject(activity), Encoding.UTF8, "application/json");
                    var response = client.PostAsync(url, c).GetAwaiter().GetResult();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "TimedHostedService - StartActivity Failed!");
            }

            _logger.Information("TimedHostedService - StartActivity Finish!");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }

}
