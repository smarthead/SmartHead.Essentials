using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartHead.Essentials.Abstractions.Cqrs;
using SmartHead.Essentials.Abstractions.MediatR;

namespace SmartHead.Essentials.Implementation.Bus
{
    public class InMemoryBus : IMediatorHandler
    {
        protected readonly IMediator Mediator;
        protected readonly IEventStore EventStore;

        public InMemoryBus(IEventStore eventStore, IMediator mediator)
        {
            EventStore = eventStore;
            Mediator = mediator;
        }

        public virtual async Task SendCommandAsync<T>(T command, CancellationToken ct = default)
            where T : Command
            => await Mediator.Send(command, ct);

        public virtual async Task RaiseEventAsync<T>(T @event, CancellationToken ct = default) 
            where T : Event
        {
            if (!@event.MessageType.Equals("DomainNotification"))
                await EventStore.SaveAsync(@event, ct);

            await Mediator.Publish(@event, ct);
        }
    }
}
