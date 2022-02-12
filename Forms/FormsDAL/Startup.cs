using AutoMapper;
using FormsDal.Services;
using Infrastructure.Auth;
using Infrastructure.Core;
using Infrastructure.Extensions;
using Infrastructure.Filters;
using Infrastructure.Formatters;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Model.Data;
using Model.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using FormsDal.Contexts;

namespace FormsDal
{
    public class Startup
    {
        private readonly IConfiguration Configuration;

        public Startup(IConfiguration config)
        {
            Configuration = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
            });

            services.AddCors(options => options.AddPolicy("ApiCorsPolicy", builder =>
            {
                builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            }));

            services.Configure<KestrelServerOptions>(Configuration.GetSection("Kestrel"));


            // configurations
            services.AddSingleton(Configuration);
            services.AddOptions(); //Add Support for strongly typed Configuration and map to class

            // appConfig and authOptions
            var configSection = Configuration.GetSection("AppConfig");
            var appConfig = configSection?.Get<AppConfig>();
            services.AddSingleton<IAppConfig>(appConfig);

            // authOptions
            var authConfigSection = Configuration.GetSection("AuthOptions");
            var authOptions = authConfigSection?.Get<AuthOptions>();
            services.AddSingleton<IAuthOptions>(authOptions);

            var prefixSection = Configuration.GetSection("DBPrefix");
            var prefix = prefixSection?.Get<DBPrefix>();
            services.AddSingleton<IDBPrefix>(prefix);

            // healths
            services.AddHealthChecks();

            // contexts
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            // REST http clients
            services.AddHttpClient<GeneralHttpClient>("CalcApi",
                httpClient =>
                {
                    var endpoint = Configuration.GetSection("AppConfig:Endpoints:CalcApi").Value;
                    httpClient.BaseAddress = new Uri(endpoint);
                    httpClient.DefaultRequestHeaders.Add("User-Agent", appConfig.Domain);
                    httpClient.DefaultRequestHeaders.Add("X-Named-Client", "CalcApi");
                    httpClient.DefaultRequestHeaders.AddFromRequest("Authorization");
                    httpClient.DefaultRequestHeaders.AddFromRequest("Accept-Language");
                    var isCustomTimeout = double.TryParse(appConfig.ResponseTimeout, out double apiResponseTimeout);
                    httpClient.Timeout = TimeSpan.FromMinutes(isCustomTimeout ? apiResponseTimeout : 10);
                });

            // Authorization
            services.AddAuthorization(options =>
            {
                options.AddPolicy(PolicyTypes.ApiAuthPolicy, policy =>
                    policy.Requirements.Add(new TokenRequirement(authOptions)));
            });
            services.AddSingleton<IAuthorizationHandler, ApiAuthorizationHandler>();

            // Current referenced assemblies for search validators and mappers
            List<Assembly> referencedAssemblies = Assembly.GetEntryAssembly().GetReferencedAssemblies()
                .Where(x => x.FullName.StartsWith("Model,"))
                .Select(x => Assembly.Load(x.FullName))
                .ToList();
            referencedAssemblies.Add(Assembly.GetEntryAssembly());

            // mvc
            services
                .AddMvc(options =>
                {
                    options.InputFormatters.Insert(0, new RawRequestBodyFormatter());
                    options.Filters.Add(typeof(ApiExceptionFilter));
                    options.Filters.Add(typeof(ApiValidationFilter));
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddFluentValidation(config =>
                {
                    config.RegisterValidatorsFromAssemblies(referencedAssemblies);
                    config.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                });

            services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = long.MaxValue;
            });

            // model state validation
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.UseMemberCasing();
                })
                .AddJsonOptions(jsonOptions =>
                {
                    jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                });

            // authentication
            services.ConfigureApiAuthentication(authOptions);

            // cache
            services.AddCacheServices();

            // background
            //services.AddBackgroundServices();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v2", new OpenApiInfo { Title = "Data Access Layer WebAPI", Version = "v 2.0.0" });

                if (authOptions.AuthenticationType == "Bearer")
                {
                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme.",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "BearerScheme",
                        BearerFormat = "JWT"
                    });
                    c.OperationFilter<ApiAuthorizationOperationFilter>();
                    c.OperationFilter<ApiSwaggerOperationNameFilter>();
                    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            Array.Empty<string>()
                        }
                    });
                }
            });

            // automapper
            services.AddAutoMapper(referencedAssemblies);

            services.Configure<RequestLocalizationOptions>(
                options =>
                {
                    var supportedCultures = new List<CultureInfo>
                    {
                        new CultureInfo("en-US"),
                        new CultureInfo("he-IL")
                    };

                    options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");
                    options.SupportedCultures = supportedCultures;
                    options.SupportedUICultures = supportedCultures;
                });
            services.AddLocalization();

            // logic services

            services.AddScoped<GeneralService>();
            //services.AddScoped<FormsSurveyDBManager>();
            services.AddScoped<FormsDBServices>();
            services.AddScoped<EventService>();
            
            services.AddScoped(serviceProvider =>
            {
                IStringLocalizerFactory factory = serviceProvider.GetService<IStringLocalizerFactory>();

                var connectionString = Configuration.GetConnectionString("FormsManageDB");
                var builder = new DbContextOptionsBuilder<DbContext>();
                var env = serviceProvider.GetService<IWebHostEnvironment>();

                // migrations
                var migrationsAssemblyName = env.IsProduction() || env.IsDevelopment()
                    ? Assembly.GetEntryAssembly().GetName().Name
                    : $"{typeof(Startup).Assembly.GetName().Name}.{env.EnvironmentName}";

                 builder.UseNpgsql(connectionString, x => x.MigrationsAssembly(migrationsAssemblyName));
#if DEBUG
                //log for db queries, use serilog class implementing ILoggerFactory
                builder.UseLoggerFactory(serviceProvider.GetService<ILoggerFactory>());
#endif
                FormsManageDBContext dbContext = new FormsManageDBContext(builder.Options);
                return dbContext;
            });

//            services.AddScoped(serviceProvider => 
//            {
//                IStringLocalizerFactory factory = serviceProvider.GetService<IStringLocalizerFactory>();

                
//                var builder = new DbContextOptionsBuilder<DbContext>();
//                var env = serviceProvider.GetService<IWebHostEnvironment>();
//                string baseName = "Survey";



//                // migrations
//                var migrationsAssemblyName = env.IsProduction() || env.IsDevelopment()
//                    ? Assembly.GetEntryAssembly().GetName().Name
//                    : $"{typeof(Startup).Assembly.GetName().Name}.{env.EnvironmentName}";

//                string connectionString = Configuration.GetConnectionString("FormsSurveyDBStart") + GeneralContext.GetService<IDBPrefix>().FormsDBPrefix + baseName;

//                builder.UseNpgsql(connectionString, x => x.MigrationsAssembly(migrationsAssemblyName));
//#if DEBUG
//                //log for db queries, use serilog class implementing ILoggerFactory
//                builder.UseLoggerFactory(serviceProvider.GetService<ILoggerFactory>());
//#endif
//                FormsDynamicDBContext dbContext = new FormsDynamicDBContext(builder.Options);
//                return dbContext;
//            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            GeneralContext.SetServiceProvider(app.ApplicationServices);

            app.UseResponseCompression();

            // migrations
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<FormsManageDBContext>();

                //var dbContext2 = serviceScope.ServiceProvider.GetService<FormsDynamicDBContext>();

                // ensure create database
                GeneralContext.Logger.Information("Database creating...");
                var isCreated = dbContext.Database.EnsureCreated();
                GeneralContext.Logger.Information(isCreated ? "Database created successfully" : "Database already existed");

                var pendingMigrations = dbContext.Database.GetPendingMigrations().ToList();

                // apply pending migrations
                if (pendingMigrations.Any())
                {
                    try
                    {
                        GeneralContext.Logger.Warning($"Migrations: {string.Join(", ", pendingMigrations)} starting ...");
                        dbContext.Database.Migrate();
                    }
                    catch (Exception ex)
                    {
                        GeneralContext.LastErrors.AddError(ex);
                    }
                    finally
                    {
                        // Not applied pending migrations
                        var notAppliedPendingMigrations = dbContext.Database.GetPendingMigrations();
                        if (notAppliedPendingMigrations.Any())
                            GeneralContext.Logger.Warning($"\n\nMigrations {string.Join(", ", notAppliedPendingMigrations)} not applied!\n");
                        else
                            GeneralContext.Logger.Warning($"\n\nMigrations {string.Join(",", pendingMigrations)} applied successfully!\n");

                        if (GeneralContext.LastErrors.Any())
                        {
                            // Not critical migration's errors
                            var notCriticalmigrationsErrors = GeneralContext.LastErrors.Where(err => err.GetType() == typeof(string) || err.GetType() == typeof(ApiError) || err.GetType()?.BaseType == typeof(ApiError));
                            if (notCriticalmigrationsErrors.Any())
                                GeneralContext.Logger.Warning($"\n\nMigration's not critical errors: {string.Join(", ", notCriticalmigrationsErrors.Select(err => err.ToString()))}");

                            // Critical migration's errors
                            var criticalmigrationsErrors = GeneralContext.LastErrors.Where(err => err.GetType().BaseType == typeof(Exception) || err.GetType().BaseType == typeof(SystemException));
                            if (criticalmigrationsErrors.Any())
                            {
                                GeneralContext.Logger.Error($"\n\nMigration's critical errors: {string.Join(", ", criticalmigrationsErrors.Select(err => err.ToString()))}");
                                //GeneralContext.LastErrors.RemoveAll(err => err.GetType().BaseType == typeof(Exception) || err.GetType().BaseType == typeof(SystemException));
                            }
                        }
                    }
                }
            }

            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = "application/json";
                    Dictionary<string, object> checks = new Dictionary<string, object>();

                    if (report.Entries.Count > 0)
                    {
                        checks = report.Entries.Values.FirstOrDefault()
                            .Data.ToDictionary(x => x.Key, x => x.Value);
                    }

                    checks.Add("Endpoint", context.Request.Scheme + Uri.SchemeDelimiter + context.Request.Host.Value);

                    var settings = new JsonSerializerSettings();
                    settings.Converters.Add(new StringEnumConverter());
                    settings.Converters.Add(new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });

                    await context.Response.WriteAsync(
                        JsonConvert.SerializeObject(HealthCheckResult.Healthy($"{Assembly.GetEntryAssembly().GetName()}", checks), Formatting.Indented, settings));
                }
            });

            app.UseApiErrorHandler();
            app.UseApiAuthentication();
            app.UseApiContext();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseCors("ApiCorsPolicy");

            //app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = true;
            });
            app.UseSwaggerUI(c =>
            {
                c.IndexStream = () => GetType().Assembly.GetManifestResourceStream("FormsDal.api.index.html");
                c.RoutePrefix = "api";
                c.SwaggerEndpoint("/swagger/v2/swagger.json", $"{Assembly.GetEntryAssembly().GetName().Name}");
                c.DocumentTitle = "4cast Data Access Layer WebAPI";
                c.OAuthUsePkce();
                c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
                c.EnableFilter();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("/", context =>
                {
                    context.Response.Redirect("api/index.html");
                    return Task.CompletedTask;
                });
            });
        }
    }
}
