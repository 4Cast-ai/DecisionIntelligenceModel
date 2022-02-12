using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Controllers;
using Infrastructure.Interfaces;
using Infrastructure.Core;

namespace Dal
{
    /// <summary> BaseAction for all actions </summary>
    public class ControllerBaseAction : GeneralControllerBase
    {
        [NonAction]
        public TService GetService<TService>() where TService : IBaseService
        {
            var service = GeneralContext.GetService<TService>();
            service.IsTransactionEnabled = service.CurrentHttpMethod != HttpMethod.Get;
            return service;
        }

    }
}
