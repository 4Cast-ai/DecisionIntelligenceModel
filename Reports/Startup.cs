using AutoMapper;
using Infrastructure.Auth;
using Infrastructure.Core;
using Infrastructure.Extensions;
using Infrastructure.Filters;
using Infrastructure.Formatters;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Reports.Infrastructure.Resources;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
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

namespace Reports
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
            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
            });

            services.Configure<KestrelServerOptions>(Configuration.GetSection("Kestrel"));

            services.AddCors(options => options.AddPolicy("ApiCorsPolicy", builder =>
            {
                builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            }));
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

            // healths
            services.AddHealthChecks();

            // contexts
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            // http clients
            services.AddHttpClient("CalcApi", Configuration);
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
            //.AddDataAnnotationsLocalization(options =>
            //{
            //    options.DataAnnotationLocalizerProvider = (type, factory) =>
            //    {
            //        if (appConfig.Language == "en")
            //        {
            //            CultureInfo.CurrentCulture = new CultureInfo("en-Us");
            //            type = typeof(GlobalResourceEn);
            //        }
            //        else
            //        {
            //            CultureInfo.CurrentCulture = new CultureInfo("he-IL");
            //            type = typeof(GlobalResourceHe);
            //        }

            //        var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
            //        var resourceName = type.Namespace + "." + type.Name;
            //        var localizer = factory.Create(resourceName, assemblyName.Name);

            //        return localizer;
            //    };
            //});

            // mvc controllers
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
                c.SwaggerDoc("v2", new OpenApiInfo { Title = "Reports Service WebAPI", Version = "v 2.0.0" });

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

            // localization
            services.AddLocalization();
            services.AddScoped(sp =>
            {
                Type type;
                IStringLocalizerFactory factory = sp.GetService<IStringLocalizerFactory>();
                IHttpContextAccessor httpContextAccessor = sp.GetService<IHttpContextAccessor>();
                string language = httpContextAccessor.HttpContext.Request.Headers["Accept-Language"];

                if (!string.IsNullOrEmpty(language) && language.StartsWith("he-IL"))
                {
                    CultureInfo.CurrentCulture = new CultureInfo("he-IL");
                    type = typeof(GlobalResourceHe);
                }
                else
                {
                    CultureInfo.CurrentCulture = new CultureInfo("en-Us");
                    type = typeof(GlobalResourceEn);
                }

                var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
                var resourceName = type.Namespace + "." + type.Name;
                var localizer = factory.Create(resourceName, assemblyName.Name);

                return localizer;
            });
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
                c.IndexStream = () => GetType().Assembly.GetManifestResourceStream("Reports.api.index.html");
                c.RoutePrefix = "api";
                c.SwaggerEndpoint("/swagger/v2/swagger.json", $"{Assembly.GetEntryAssembly().GetName().Name}");
                c.DocumentTitle = "Reports WebAPI";
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

            var localizationOption = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(localizationOption.Value);

            GeneralContext.SetServiceProvider(app.ApplicationServices);
        }
    }
}
