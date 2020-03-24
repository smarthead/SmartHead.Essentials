using System;
using MediatR;
using SmartHead.Essentials.Abstractions.Behaviour;

namespace SmartHead.Essentials.Abstractions.Cqrs
{
    public abstract class Event : Message, INotification, IHasCreationTime
    {
        protected Event()
        {
            CreationTime = DateTime.UtcNow;
        }

        public DateTime CreationTime { get; set; }
    }
}