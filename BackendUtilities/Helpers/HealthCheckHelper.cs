﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json.Linq;
using System.Net.Mime;

namespace Infrastructure.Helpers
{
    public static class HealthCheckHelper
    {
        public static async Task<HealthCheckResult> GenerateHealthCheckResultFromPingRequest(string hostName)
        {
            using (var thePing = new Ping())
            {
                var pingResult = await thePing.SendPingAsync(hostName);
                var description = $"A ping of the {hostName} host";
                var healthCheckData = new Dictionary<string, object>();

                // Gets the number of milliseconds taken to send an Internet Control Message Protocol
                // (ICMP) echo request and receive the corresponding ICMP echo reply message.
                healthCheckData.Add("RoundtripTime", pingResult.RoundtripTime);

                healthCheckData.Add("ActualIPAddress", pingResult.Address.ToString());

                if (pingResult.Status == IPStatus.Success)
                {
                    return HealthCheckResult.Healthy(description, healthCheckData);
                }

                return HealthCheckResult.Unhealthy(description, null, healthCheckData);
            }
        }

        public static async Task WriteResponses(HttpContext context, HealthReport result)
        {
            var json = new JObject(
                            new JProperty("status", result.Status.ToString()),
                            new JProperty("results", new JObject(result.Entries.Select(pair =>
                            new JProperty(pair.Key, new JObject(
                                new JProperty("status", pair.Value.Status.ToString()),
                                new JProperty("description", pair.Value.Description),
                                new JProperty("data", new JObject(pair.Value.Data.Select(
                                    p => new JProperty(p.Key, p.Value))))))))));

            context.Response.ContentType = MediaTypeNames.Application.Json;
            await context.Response.WriteAsync(json.ToString(Formatting.Indented));
        }
    }
}
