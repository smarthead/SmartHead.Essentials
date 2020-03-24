using System;

namespace SmartHead.Essentials.Abstractions.Cqrs
{
    public class DomainNotification : Event
    {
        public DomainNotification(string key, string value = default)
        {
            DomainNotificationId = Guid.NewGuid();
            Key = key;
            Value = value;
        }

        public Guid DomainNotificationId { get; }
        public string Key { get; }
        public string Value { get; }
    }
}