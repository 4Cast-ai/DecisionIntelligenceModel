using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading;

namespace Infrastructure.Interfaces
{
    public class IContextServiceBase<T> where T : DbContext
    {
        T DbContext { get; set; }

        // Transactions
        QueryTrackingBehavior QueryTrackingBehavior { get; set; }
        IDbContextTransaction Transaction { get; }
        bool? IsTransactionEnabled { get; set; }
        bool IsTransactionOwner { get; }
        bool IsDbContextOwner { get; }
        CancellationToken CancelToken { get; set; }
    }
}
