using Microsoft.AspNetCore.Mvc.Filters;
using Infrastructure.Core;
using Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Interfaces;
using Model.Entities;

namespace Infrastructure.Filters
{
    public class UnitOfWorkAttribute : IActionFilter
    {
        public IUnitOfWork<Context> UoW { get; set; }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            //throw new System.NotImplementedException();
        }

        public  void OnActionExecuting(ActionExecutingContext context)
        {
            UoW = GeneralContext.GetService(typeof(IUnitOfWork<Context>)) as IUnitOfWork<Context>;
            System.Diagnostics.Trace.WriteLine("Scoped UoW " + UoW.SessionId);

            //Uow.Commit(); here you would commit
            System.Diagnostics.Trace.WriteLine("GlobalConfig UoW " + UoW.SessionId);
        }
    }
}
