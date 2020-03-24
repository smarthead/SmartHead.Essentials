using SmartHead.Essentials.Abstractions.Ddd.Interfaces;

namespace SmartHead.Essentials.Abstractions.Cqrs
{
    public class StoredEvent : Event, IEntity
    {
        public StoredEvent(Message message, string data, string user)
        {
            AggregateId = message.AggregateId;
            MessageType = message.MessageType;
            Data = data;
            User = user;
        }

        protected StoredEvent()
        {
        }

        public string Data { get; protected set; }
        public string User { get; protected set; }
        public long Id { get; protected set; }
    }
}