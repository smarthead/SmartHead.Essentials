using System.Threading;
using System.Threading.Tasks;
using SmartHead.Essentials.Abstractions.Cqrs;

namespace SmartHead.Essentials.Abstractions.MediatR
{
    public interface IEventStore
    {
        Task SaveAsync<T>(T @event, CancellationToken ct = default) where T : Event;
    }
}
