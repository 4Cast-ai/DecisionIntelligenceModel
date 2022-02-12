using AutoMapper;
using Infrastructure.Auth;
using Infrastructure.Core;
using Infrastructure.Extensions;
using Infrastructure.Filters;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Infrastructure.Middleware;

namespace Expert
{
    public class Startup
    {
        private readonly IConfiguration Configuration;

        public Startup(IWebHostEnvironment env, IConfiguration config)
        {
            Configuration = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
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

            // appConfig
            var configSection = Configuration.GetSection("AppConfig");
            var appConfig = configSection?.Get<AppConfig>();
            services.AddSingleton<IAppConfig>(appConfig);

            // authOptions
            var authConfigSection = Configuration.GetSection("AuthOptions");
            var authOptions = authConfigSection?.Get<AuthOptions>();
            services.AddSingleton<IAuthOptions>(authOptions);

            // healths
            services.AddHealthChecks();

            // contexts
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            // http clients
            services.AddHttpClient("InterfaceApi", Configuration);
            services.AddHttpClient("DalApi", Configuration);

            // authorization
            services.AddAuthorization(options =>
            {
                options.AddPolicy(PolicyTypes.ApiAuthPolicy, policy =>
                    policy.Requirements.Add(new TokenRequirement(authOptions)));
            });
            services.AddSingleton<IAuthorizationHandler, ApiAuthorizationHandler>();

            // current referenced assemblies for search validators and mappers
            List<Assembly> referencedAssemblies = Assembly.GetEntryAssembly().GetReferencedAssemblies()
                .Where(x => x.FullName.StartsWith("Model,"))
                .Select(x => Assembly.Load(x.FullName))
                .ToList();
            referencedAssemblies.Add(Assembly.GetEntryAssembly());

            // mvc
            services
                .AddMvc(options =>
                {
                    options.Filters.Add(typeof(ApiExceptionFilter));
                    options.Filters.Add(typeof(ApiValidationFilter));
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddFluentValidation(config =>
                {
                    config.RegisterValidatorsFromAssemblies(referencedAssemblies);
                    config.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                });

            // mvc controller
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

            // model state validation
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            // authentication
            services.ConfigureApiAuthentication(authOptions);

            services.AddMemoryCache();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v2", new OpenApiInfo { Title = "Expert Service WebAPI", Version = "v 2.0.0" });

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
            services.AddAutoMapper(referencedAssemblies);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseResponseCompression();

            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = "application/json";
                    Dictionary<string, object> checks = new Dictionary<string, object>();

                    if (report.Entries.Count() > 0)
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

            //app.UseApiErrorHandler();
            //app.UseApiAuthentication();
            //app.UseApiContext();

            app.UseMiddleware<ApiErrorHandlingMiddleware>();
            app.UseMiddleware<ApiAuthenticationMiddleware>();
            app.UseMiddleware<ApiContextMiddleware>(); ;


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
                c.IndexStream = () => GetType().Assembly.GetManifestResourceStream("Expert.api.index.html");
                c.RoutePrefix = "api";
                c.SwaggerEndpoint("/swagger/v2/swagger.json", $"{Assembly.GetEntryAssembly().GetName().Name}");
                c.DocumentTitle = "Expert WebAPI";
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

            GeneralContext.SetServiceProvider(app.ApplicationServices);
        }
    }
}
