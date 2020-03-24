using System.Threading.Tasks;
using MediatR;
using SmartHead.Essentials.Abstractions.Cqrs;
using SmartHead.Essentials.Abstractions.Ddd.Interfaces;
using SmartHead.Essentials.Abstractions.MediatR;
using SmartHead.Essentials.Implementation.Events;

namespace SmartHead.Essentials.Implementation.Commands
{
    public class CommandHandlerBase
    {
        protected readonly IMediatorHandler Mediator;
        protected readonly DomainNotificationHandler Notifications;
        protected readonly IUnitOfWork UoW;

        public CommandHandlerBase(
            IUnitOfWork uow,
            IMediatorHandler mediator,
            INotificationHandler<DomainNotification> notifications)
        {
            UoW = uow;
            Notifications = (DomainNotificationHandler)notifications;
            Mediator = mediator;
        }

        public virtual bool Commit()
        {
            if (Notifications.HasNotifications()) 
                return false;

            if (UoW.Commit()) 
                return true;

            Mediator.RaiseEventAsync(new DomainNotification(nameof(DomainNotification), "Commit failed.")).GetAwaiter().GetResult();
            return false;
        }

        public virtual async Task<bool> CommitAsync()
        {
            if (Notifications.HasNotifications()) 
                return false;

            if (await UoW.CommitAsync())
                return true;

            await Mediator.RaiseEventAsync(new DomainNotification(nameof(DomainNotification), "Commit failed."));
            return false;
        }
    }
}
