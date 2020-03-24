using MediatR;

namespace SmartHead.Essentials.Abstractions.Cqrs
{
    public abstract class Message : IRequest<bool>
    {
        protected Message()
        {
            MessageType = GetType().Name;
        }

        public string MessageType { get; protected set; }
        public long AggregateId { get; protected set; }
    }
}