using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartHead.Essentials.Abstractions.Cqrs;

namespace SmartHead.Essentials.Abstractions.MediatR
{
    public abstract class NotificationHandlerBase<T> : INotificationHandler<T>
        where T : Event
    {
        private readonly List<T> _notifications;

        protected NotificationHandlerBase()
        {
            _notifications = new List<T>();
        }

        public Task Handle(T message, CancellationToken cancellationToken)
        {
            _notifications.Add(message);
            return Task.CompletedTask;
        }

        public virtual List<T> Notifications() => _notifications;
        public virtual T LastEvent() => Notifications().Last();
        public virtual bool HasNotifications() => Notifications().Any();
    }
}