using System;
using SmartHead.Essentials.Abstractions.Behaviour;

namespace SmartHead.Essentials.Abstractions.Cqrs
{
    public abstract class Command : Message, IHasCreationTime
    {
        protected Command()
        {
            CreationTime = DateTime.UtcNow;
        }

        public DateTime CreationTime { get; set; }
    }
}