using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Core;
using Infrastructure.DataContext;
using Infrastructure.Enums;
using Infrastructure.Extensions;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Model.Entities;
using FormsDal.Contexts;

namespace Infrastructure.Services
{
    public abstract class BaseService :  IDisposable, IBaseService
    {
        private readonly CancellationTokenSource _cts = new CancellationTokenSource(); // Create the token source.
        private readonly Lazy<GeneralHttpClient> _CalcClient = new Lazy<GeneralHttpClient>(() => GeneralContext.CreateRestClient(ApiServiceNames.CalcApi));

        /// <summary> Current Calculator http rest client for access to CalculatorApi </summary>
        public GeneralHttpClient CalcApi => _CalcClient.Value;

        #region Public properties

        //public bool UseNewContext { get; set; } = false;

        private IBaseService _parent;
        public virtual IBaseService Parent
        {
            get { return _parent; }
            set
            {
                _parent = value;
                //this.DbContext = value.DbContext;
                //this.QueryTrackingBehavior = value.QueryTrackingBehavior;
                //this.IsTransactionEnabled = value.IsTransactionEnabled;
            }
        }

        /// <summary> Action's cancellation token </summary>
        public CancellationToken CancelToken { get; set; } = default;


        /// <summary> Request absolute path of sender </summary>
        public string RequestUrl
        {
            get
            {
                var urlHelperFactory = GeneralContext.GetService<IUrlHelperFactory>();
                var actionContextAccessor = GeneralContext.GetService<IActionContextAccessor>();
                if (actionContextAccessor.ActionContext != null)
                {
                    var urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
                    return urlHelper.GetAbsoluteUrl();
                }
                else
                {
                    return HttpContext.Request.Path;
                }
            }
        }

        public HttpContext HttpContext => GeneralContext.HttpContext;

        public IAuthOptions AuthOptions => GeneralContext.GetService<IAuthOptions>();

        /// <summary> Current HttpMethod on create service</summary>
        public HttpMethod CurrentHttpMethod
        {
            get
            {
                if (string.IsNullOrEmpty(HttpContext.Request.Method))
                    return HttpMethod.Get;
                return new HttpMethod(HttpContext.Request.Method);
            }
        }

        public bool IsAjaxRequest => HttpContext.Request.Headers["x-requested-with"] == "XMLHttpRequest";

       

        public IMapper Mapper => GeneralContext.GetService<IMapper>();

        #endregion Public properties

        #region Create child services

        /// <summary> Create child service and set this as parent </summary>
        public virtual TService GetChildService<TService>() where TService : IBaseService
        {
            TService service = default;
            try
            {
                service = HttpContext.RequestServices.GetRequiredService<TService>();
                service.Parent = this;
                //service.IsTransactionEnabled = this.IsTransactionEnabled;
            }
            catch (Exception ex)
            {
                var errorMessage = $"Serice {typeof(TService).Name} not registred {ex.GetApiMessageInfo()}";
                throw new InvalidOperationException(GeneralContext.GetApiMessageInfo(errorMessage, EventLevel.Critical));
            }
            return service;
        }

        /// <summary> Create child repository and set this as parent </summary>
        //public virtual TRepository GetChildRepository<TRepository>() where TRepository : IBaseRepository<T>
        //{
        //    TRepository repo = HttpContext.RequestServices.GetRequiredService<TRepository>();
        //    if (repo != null)
        //    {
        //        repo.Parent = this;
        //    }
        //    else
        //    {
        //        var errorMessage = $"Repository {typeof(TRepository).Name} not registred";
        //        throw new InvalidOperationException(GeneralContext.GetApiMessageInfo(errorMessage, EventLevel.Critical));
        //    }
        //    return repo;
        //}

        /// <summary> Create service in root scope </summary>
        public TService GetRootService<TService>() where TService : BaseService
        {
            var _serviceScope = GeneralContext.CreateServiceScope();
            var service = GeneralContext.GetService<TService>();
            //service.DbContext = _serviceScope.ServiceProvider.GetService<T>();
            return service as TService;
        }

        public TService GetChildScopeService<TService>() where TService : BaseService
        {
            var _serviceScope = GeneralContext.CreateServiceScope();
            var service = _serviceScope.ServiceProvider.GetService<TService>();
            //service.DbContext = _serviceScope.ServiceProvider.GetService<T>();
            return service as TService;
        }

        #endregion

        #region Ok/Error results

        /// <summary> Commit transaction, if IsTransactionEnabledEnabled=true, and try return HttpStatusCode.OK </summary>
        public Task<ServiceResult<bool>> OkResult()
        {
            return OkResult(true, HttpStatusCode.OK);
        }
        /// <summary> Commit transaction, if IsTransactionEnabledEnabled=true, and try return HttpStatusCode.OK </summary>
        public Task<ServiceResult<T>> OkResult<T>(T value, HttpStatusCode statusCode = HttpStatusCode.OK, string message = null)
        {
            //if (IsTransactionEnabled.GetValueOrDefault() && statusCode == HttpStatusCode.OK)
            //{
            //    var committedChangesCount = this.CommitTransaction();
            //    message = $"{message ?? ""} {committedChangesCount} savechanges committed";
            //    statusCode = committedChangesCount > 0 ? HttpStatusCode.OK : HttpStatusCode.NotModified;

            //    if (typeof(T) == typeof(HttpStatusCode))
            //    {
            //        statusCode = HttpStatusCode.OK;
            //        message += "; value contains statusCode";
            //    }
            //}

            var result = new ServiceResult<T>(value, statusCode, message) { RequestUrl = RequestUrl };
            return Task.FromResult(result);
        }
        /// <summary> Commit transaction, if IsTransactionEnabledEnabled=true, and try return HttpStatusCode.OK </summary>
        public Task<ServiceResult<TEntity>> OkResult<TEntity>(EntityEntry<TEntity> result) where TEntity : class
        {
            HttpStatusCode statusCode = result.State != EntityState.Unchanged ? HttpStatusCode.OK : HttpStatusCode.NotModified;
            return OkResult(result.Entity, statusCode, $"state: {result.State}; statusCode: {statusCode}");
        }
        /// <summary> Commit transaction, if IsTransactionEnabledEnabled=true, and try return HttpStatusCode.OK </summary>
        public Task<ServiceResult<bool>> OkResult(List<EntityEntry> operationResults)
        {
            var summarySuccess = operationResults.All(result => result.State != EntityState.Unchanged);
            string summaryMessage = string.Join("; ", operationResults.Select(result => result.ToString()));

            HttpStatusCode statusCode = summarySuccess ? HttpStatusCode.OK : HttpStatusCode.NotModified;
            return OkResult(summarySuccess, statusCode, summaryMessage);
        }
        /// <summary> Commit transaction, if IsTransactionEnabledEnabled=true, and try return HttpStatusCode.OK </summary>
        public Task<ServiceResult<bool>> OkResult(List<bool> operationResults)
        {
            var summarySuccess = operationResults.All(result => true);
            string summaryMessage = string.Join("; ", operationResults.Select(result => result.ToString()));

            HttpStatusCode statusCode = summarySuccess ? HttpStatusCode.OK : HttpStatusCode.InternalServerError;
            return OkResult(summarySuccess, statusCode, summaryMessage);
        }

        /// <summary> Rollback transaction, and try return HttpStatusCode.Conflict </summary>
        public Task<ServiceResult<TResult>> ErrorResult<TResult>(Exception ex)
        {
            var result = new ServiceResult<TResult>(default(TResult), HttpStatusCode.Conflict, ex?.InnerException?.Message ?? ex.Message) { RequestUrl = RequestUrl };
            //this.RollbackTransaction();
            return OkResult(default(TResult), HttpStatusCode.Conflict, result.Message);
        }
        /// <summary> Rollback transaction, and try return HttpStatusCode.Conflict </summary>
        public Task<ServiceResult<TResult>> ErrorResult<TResult>(string message = "operation error")
        {
            var result = new ServiceResult<TResult>(default(TResult), HttpStatusCode.Conflict, message) { RequestUrl = RequestUrl };
            //this.RollbackTransaction();
            return OkResult(default(TResult), HttpStatusCode.Conflict, result.Message);
        }
        /// <summary> Rollback transaction, and try return HttpStatusCode.Conflict </summary>
        public Task<ServiceResult<bool>> ErrorResult(Exception ex)
        {
            return ErrorResult(ex?.InnerException?.Message ?? ex.Message);
        }
        /// <summary> Rollback transaction, and try return HttpStatusCode.Conflict </summary>
        public Task<ServiceResult<bool>> ErrorResult(string message)
        {
            //this.RollbackTransaction();
            return OkResult(false, HttpStatusCode.Conflict, message);
        }

        /// <summary> EntityResult, by state of entityState </summary>
        public ServiceResult<bool> EntityResult<TEntity>(Task<EntityEntry<TEntity>> task) where TEntity : class
        {
            //object value = typeof(TResult) == typeof(bool) 
            //    ? Convert.ChangeType(task.Result.State != EntityState.Unchanged, typeof(TResult)) 
            //    : task.Result.Entity;

            //TResult result = (TResult)Convert.ChangeType(value, typeof(TResult));

            return new ServiceResult<bool>(task.Result.State != EntityState.Unchanged,
                task.Result.State != EntityState.Unchanged ? HttpStatusCode.OK : HttpStatusCode.NotModified);
        }

        #endregion Ok/Error results

        #region Helpers (ChildServices, ChildRepositories)

        //TODO: remove SessionId
        public string SessionId => throw new NotImplementedException();

        public string GetConnectionString()
        {
            string connectionString = GeneralContext.GetConnectionString("4CASTDatabase");

            if (string.IsNullOrEmpty(connectionString) || connectionString.Contains("{0}"))
                GeneralContext.Logger.Error("connectionString is null or empty or not valid");

            return connectionString;
        }

        //private bool isTransactionEnabled;
        private static readonly object lockingObject = new object();

        #endregion Helpers (ChildServices, ChildRepositories)



        #region Idisposable

        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    //_serviceScope?.Dispose();
                    //try
                    //{
                    //    if (_dbContext?.Database?.CurrentTransaction != null && IsTransactionEnabled && !committed)
                    //    {
                    //        var transactionId = _dbContext?.Database?.CurrentTransaction?.TransactionId;
                    //        GeneralContext.Logger.Warning($"transactionId {transactionId} not committed");
                    //        this.RollbackTransaction();
                    //    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    GeneralContext.Logger.Error(ex, $"transaction committing");
                    //}
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~BaseService()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion Idisposable
    }
}
