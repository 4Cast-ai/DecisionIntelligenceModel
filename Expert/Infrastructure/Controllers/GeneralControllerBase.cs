using AutoMapper;
using Infrastructure.Core;
using Infrastructure.Enums;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Infrastructure.Controllers
{
    /// <summary> BaseAction for all actions </summary>
    public class GeneralControllerBase : ControllerBase
    {
        #region Public properties

        /// <summary> Current mapper </summary>
        public virtual IMapper Mapper => GeneralContext.GetService<IMapper>();

        /// <summary> Current app config </summary>
        public virtual IAppConfig Appconfig => GeneralContext.GetService<IAppConfig>();

        /// <summary> Current auth options from config</summary>
        public virtual IAuthOptions AuthOptions => GeneralContext.GetService<IAuthOptions>();

        /// <summary> Current httpContext </summary>
        public new HttpContext HttpContext => GeneralContext.GetService<IHttpContextAccessor>()?.HttpContext;

        #endregion Public properties

        #region Rest api clients

        private readonly Lazy<GeneralHttpClient> _DBGate = new Lazy<GeneralHttpClient>(() => GeneralContext.CreateRestClient(ApiServiceNames.DalApi));
        /// <summary> Current DBGate http rest client for access to Db </summary>
        public GeneralHttpClient DBGate => _DBGate.Value;

        private readonly Lazy<GeneralHttpClient> _CalcClient = new Lazy<GeneralHttpClient>(() => GeneralContext.CreateRestClient(ApiServiceNames.CalcApi));
        /// <summary> Current Calculator http rest client for access to CalculatorApi </summary>
        public GeneralHttpClient CalcApi => _CalcClient.Value;

        private readonly Lazy<GeneralHttpClient> _ExpertClient = new Lazy<GeneralHttpClient>(() => GeneralContext.CreateRestClient(ApiServiceNames.ExpertApi));
        /// <summary> Current Expert http rest client for access to ExpertApi </summary>
        public GeneralHttpClient ExpertApi => _ExpertClient.Value;

        private readonly Lazy<GeneralHttpClient> _EventsClient = new Lazy<GeneralHttpClient>(() => GeneralContext.CreateRestClient(ApiServiceNames.EventsApi));
        /// <summary> Current Events http rest client for access to EventsApi </summary>
        public GeneralHttpClient EventsApi => _EventsClient.Value;

        private readonly Lazy<GeneralHttpClient> _ReportsClient = new Lazy<GeneralHttpClient>(() => GeneralContext.CreateRestClient(ApiServiceNames.ReportApi));
        /// <summary> Current Reports http rest client for access to ReportsApi </summary>
        public GeneralHttpClient ReportApi => _ReportsClient.Value;

        private readonly Lazy<GeneralHttpClient> _InterfaceClient = new Lazy<GeneralHttpClient>(() => GeneralContext.CreateRestClient(ApiServiceNames.InterfaceApi));
        /// <summary> Current EmsInterface http rest client for access to Ems interface Api </summary>
        public GeneralHttpClient InterfaceApi => _InterfaceClient.Value;

        #endregion Rest api clients
    }
}