using Infrastructure.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IReportPublisher
    {
        Task Publish(ApiReportItem reportItem, CancellationToken cancellationToken = default);
    }
}
