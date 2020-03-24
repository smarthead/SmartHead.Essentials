using System.Threading;
using System.Threading.Tasks;

namespace SmartHead.Essentials.Abstractions.Ddd.Interfaces
{
    public interface IUnitOfWork
    {
        bool Commit();
        Task<bool> CommitAsync(CancellationToken ct = default);
    }
}