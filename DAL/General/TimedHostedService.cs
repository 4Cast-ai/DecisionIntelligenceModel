using Infrastructure.Helpers;
using Infrastructure.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Model.Data;
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
                    var activities = DbContext.Activity.Where(a => a.Status.HasValue && a.Status.Value == (int)Model.Data.ActivityStatus.Draft /*&&
                                                                (Util.ConvertStringToDate(a.StartDate)).Date == DateTime.Today*/).ToList();

                    FormsDataObject data = null;
                    foreach (var activity in activities)
                    {
                        //create activity object
                        data = new FormsDataObject()
                        {
                            ActivityGuid = activity.ActivityGuid,
                            ActivityName = activity.Name,
                            StartDate = activity.StartDate,
                            EndDate = activity.EndDate,
                            IsLimited = activity.ActivityTemplateGu.WithinTimeRange,
                            CanSubmitOnce = activity.ActivityTemplateGu.SubmitOnlyOnce.Value,
                            IsAnonymous = activity.AnonymousEvaluation
                        };

                        if (activity.ActivityTemplateGu.EntityType == (int)EntityTypeEnum.Unit)
                        {//unit
                            data.EvaluatorList = (from aes in DbContext.ActivityEstimator
                                                 join p in DbContext.Person on aes.EstimatedGuid equals p.PersonGuid
                                                 join ae in DbContext.ActivityEntity on aes.ActivityEntity equals ae.ActivityEntityId
                                                 where ae.ActivityGuid == activity.ActivityGuid
                                                 select new Evaluator() { 
                                                    EvaluatorGuid = aes.EstimatedGuid,
                                                    //EvaluatorName = "",
                                                    EvaluatorType = (int)EntityTypeEnum.Person,
                                                    EvaluatorUnitGuid = "",
                                                    Email = "",
                                                    Phone = "",
                                                    EvaluatedList = null
                                                 }).ToList();
                        }
                        else
                        {//person
                            data.EvaluatorList = null;
                        }

                        using (var client = new HttpClient())
                        {
                            string url = formsApiUrl + "api/FormsHandlerApi/Event/CreateEvent";
                            HttpContent c = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                            var response = client.PostAsync(url, c).GetAwaiter().GetResult();
                        }

                        activity.Status = (int)Model.Data.ActivityStatus.InProcess;
                        DbContext.Activity.Update(activity);
                        DbContext.SaveChanges();
                    }
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
