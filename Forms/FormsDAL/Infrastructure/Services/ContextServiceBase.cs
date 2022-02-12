using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net.Http;

namespace Infrastructure.Services
{
    public class ContextServiceBase<T> where T : DbContext
    {
        public T _dbContext = null;
        public T DbContext
        {
            get
            {
                if (_dbContext == null)
                {

                    _dbContext = GeneralContext.GetService<T>();

                    var _isTransactionEnabled = IsTransactionEnabled.GetValueOrDefault();

                    QueryTrackingBehavior = _isTransactionEnabled
                        ? QueryTrackingBehavior.TrackAll : QueryTrackingBehavior.NoTracking;

                    if (_isTransactionEnabled)
                        ForceBeginTransaction();

                    IsDbContextOwner = true;
                }
                return _dbContext;
            }
            set
            {
                _dbContext = value
            }
        }
        public HttpMethod CurrentHttpMethod
        {
            get
            {
                if (string.IsNullOrEmpty(HttpContext.Request.Method))
                    return HttpMethod.Get;
                return new HttpMethod(HttpContext.Request.Method);
            }
        }

        #region Public properties
        /// <summary> The change tracker will not track any of the entities that are returned from a LINQ query /// </summary>
        private QueryTrackingBehavior _queryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        public QueryTrackingBehavior QueryTrackingBehavior
        {
            get { return _queryTrackingBehavior; }
            set
            {
                _queryTrackingBehavior = value;
                _dbContext.ChangeTracker.QueryTrackingBehavior = _queryTrackingBehavior;
            }
        }
        /// <summary> Get current transaction from Database </summary>
        public IDbContextTransaction Transaction => DbContext?.Database?.CurrentTransaction;

        /// <summary> Is owner who created DbContext </summary>
        public bool IsDbContextOwner { get; private set; }
        /// <summary> Is owner who created DbContext </summary>
        public bool IsTransactionOwner { get; private set; }

        public bool IsAutoSaveChangesEnabled { get; private set; }

        private bool? _isTransactionEnabled;
        public bool? IsTransactionEnabled
        {
            get
            {
                if (!_isTransactionEnabled.HasValue)
                {
                    _isTransactionEnabled = CurrentHttpMethod != HttpMethod.Get;
                    IsAutoSaveChangesEnabled = _isTransactionEnabled.GetValueOrDefault();
                }
                return _isTransactionEnabled;
            }

            set
            {
                _isTransactionEnabled = value;
                IsAutoSaveChangesEnabled = _isTransactionEnabled.GetValueOrDefault();
            }
        }


        #endregion


        #region IUnitOfWork

        public void ResetDbContext()
        {
            _dbContext = null;
        }

        // TODO: Implement
        public DbSet<T> Set<T>() where T : class
        {
            throw new NotImplementedException();
        }

        public virtual void ForceBeginTransaction()
        {
            lock (lockingObject)
            {
                if (_dbContext.Database.CurrentTransaction == null)
                {
                    var _transaction = _dbContext.Database.BeginTransaction();
                    IsTransactionOwner = _transaction != null;
                    CancelToken = _cts.Token;
                }
            }
        }

        public virtual int CommitTransaction()
        {
            string transactionId = "";
            int changesCount = 0;
            try
            {
                changesCount = DbContext.GetChangesCounter();
                if (changesCount == 0 && IsAutoSaveChangesEnabled)
                    changesCount = DbContext.SaveChanges();

                if (_dbContext.Database.CurrentTransaction != null && !CancelToken.IsCancellationRequested)
                {
                    transactionId = _dbContext.Database.CurrentTransaction.TransactionId.ToString();
                    _dbContext.Database.CurrentTransaction.Commit();
                    GeneralContext.Logger.Information($"transaction {transactionId} committed");
                }

                _dbContext.ClearChangesCounter();
            }
            catch (Exception ex)
            {
                GeneralContext.Logger.Error($"transaction {transactionId} not committed, because: \n{(ex.InnerException ?? ex).Message}");

                if (_dbContext.Database.CurrentTransaction != null)
                    _dbContext.Database.CurrentTransaction.Rollback();
                _cts.Cancel();
            }

            return changesCount;
        }

        public virtual async Task<int> CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            string transactionId = "";
            int changesCount = 0;
            try
            {
                changesCount = DbContext.GetChangesCounter();
                if (changesCount == 0 && IsAutoSaveChangesEnabled)
                    changesCount = await DbContext.SaveChangesAsync();

                if (_dbContext.Database.CurrentTransaction != null && !CancelToken.IsCancellationRequested)
                {
                    transactionId = _dbContext.Database.CurrentTransaction.TransactionId.ToString();
                    await _dbContext.Database.CurrentTransaction.CommitAsync();
                    GeneralContext.Logger.Information($"transaction {transactionId} committed");
                }

                _dbContext.ClearChangesCounter();
            }
            catch (Exception ex)
            {
                GeneralContext.Logger.Error($"transaction {transactionId} not committed, because: \n{(ex.InnerException ?? ex).Message}");

                if (_dbContext.Database.CurrentTransaction != null)
                    await _dbContext.Database.CurrentTransaction.RollbackAsync();

                _cts.Cancel();
            }
            return changesCount;
        }

        public virtual void RollbackTransaction()
        {
            if (_dbContext?.Database?.CurrentTransaction != null)
            {
                _dbContext.Database.CurrentTransaction.Rollback();
                //_dbContext.Database.CurrentTransaction.Dispose();
                _dbContext.Dispose();
                _dbContext = null;
            }
        }

        public virtual void SetIsolationLevel(IsolationLevel isolationLevel)
        {
            throw new NotImplementedException();
        }

        public int CommitTransaction(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        #endregion IUnitOfWork

        #region SaveChanges/Commit/Rollback

        /// <summary> SaveChanges </summary>
        /// <returns>The number of state entries written to the database</returns>
        public virtual int SaveChanges()
        {
            var writtenCount = DbContext.SaveChanges();
            return writtenCount;
        }

        /// <summary> SaveChanges </summary>
        /// <returns>The number of state entries written to the database</returns>
        public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var writtenCount = await DbContext.SaveChangesAsync();
            return writtenCount;
        }

        #endregion SaveChanges/Commit/Rollback
    }
}
