using System.Threading;
using System.Threading.Tasks;
using SmartHead.Essentials.Abstractions.Cqrs;

namespace SmartHead.Essentials.Abstractions.MediatR
{
    public interface IMediatorHandler
    {
        Task SendCommandAsync<T>(T command, CancellationToken ct = default) where T : Command;
        Task RaiseEventAsync<T>(T @event, CancellationToken ct = default) where T : Event;
    }
}
