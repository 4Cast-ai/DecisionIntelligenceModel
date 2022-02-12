﻿using System;
using System.Threading.Tasks;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Infrastructure.Models;
using Infrastructure.Extensions;
using Infrastructure.Interfaces;
using Serilog;

namespace Infrastructure.Middleware
{
    public class ApiErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Serilog.ILogger _logger;
        private readonly IWebHostEnvironment _environment;

        public ApiErrorHandlingMiddleware(RequestDelegate next, IWebHostEnvironment environment, Serilog.ILogger logger)
        {
            _next = next;
            _environment = environment;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                context.Response.OnStarting(async (state) =>
                {
                    if (context.Request.Path.Value.StartsWith("/api") && context.Response.StatusCode == StatusCodes.Status400BadRequest)
                    {
                        var ex = context.Features.Get<IExceptionHandlerFeature>()?.Error;
                        if (ex != null)
                        {
                            context.Response.Clear();
                            await HandleExceptionAsync(context, new ApiException(StatusCodes.Status400BadRequest.ToString())).ConfigureAwait(false);
                        }
                    }
                    await Task.CompletedTask;
                }, new object());

                await _next(context);

                
                if (context.Request.Path.HasValue && context.Request.Path.Value.Contains("/api/") && !context.Response.HasStarted)
                {
                    //await HandleExceptionAsync(context, new ApiException("Response has not started"));
                }
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            ApiResponse response = context.GetResponseDetails(ex);
            string json = JsonConvert.SerializeObject(response);

            if (context.Response.ContentType == null)
                context.Response.ContentType = "application/json";

            _logger.Error($"{nameof(ApiErrorHandlingMiddleware)}: ", json);

            await context.Response.WriteAsync(json);
        }
    }
}
