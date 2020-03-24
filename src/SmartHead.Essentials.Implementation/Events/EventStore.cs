using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SmartHead.Essentials.Abstractions.Behaviour;
using SmartHead.Essentials.Abstractions.Cqrs;
using SmartHead.Essentials.Abstractions.Ddd.Interfaces;
using SmartHead.Essentials.Abstractions.MediatR;

namespace SmartHead.Essentials.Implementation.Events
{
    public class EventStore : IEventStore
    {
        protected readonly IUser User;
        protected readonly IUnitOfWork Uow;
        protected readonly DbContext Context;

        public EventStore(IUser user, IUnitOfWork uow, DbContext context)
        {
            User = user;
            Uow = uow;
            Context = context;
        }

        public virtual async Task SaveAsync<T>(T @event, CancellationToken ct = default) 
            where T : Event
        {
            var data = JsonConvert.SerializeObject(@event);

            var storedEvent = new StoredEvent(
                @event,
                data,
                User.Name);

            await Context.Set<StoredEvent>().AddAsync(storedEvent, ct);
            await Uow.CommitAsync(ct);
        }
    }
}
