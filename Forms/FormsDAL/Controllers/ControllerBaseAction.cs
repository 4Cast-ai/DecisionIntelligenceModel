using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Controllers;
using Infrastructure.Interfaces;
using Infrastructure.Core;
using FormsDal.Contexts;

namespace FormsDal
{
    /// <summary> BaseAction for all actions </summary>
    public class ControllerBaseAction : GeneralControllerBase
    {
        [NonAction]
        public TService GetService<T, TService>() 
            where T : BaseContext
            where TService : IBaseService
        {
            var service = GeneralContext.GetService<TService>();
            //service.IsTransactionEnabled = service.CurrentHttpMethod != HttpMethod.Get;
            return service;
        }

    }
}
