using System;
using System.Collections.Generic;
using SmartHead.Essentials.Abstractions.Ddd.Interfaces;

namespace SmartHead.Essentials.Abstractions.Ddd
{
    public abstract class Entity : IEntity
    {
        public long Id { get; protected set; }

        public override bool Equals(object obj)
        {
            var compareTo = obj as Entity;

            if (ReferenceEquals(this, compareTo)) return true;
            return !(compareTo is null) && Id.Equals(compareTo.Id);
        }

        public static bool operator ==(Entity a, Entity b)
        {
            if (a is null && b is null)
                return true;

            if (a is null || b is null)
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Entity a, Entity b) => !(a == b);
        public override int GetHashCode() => GetType().GetHashCode() * 907 + Id.GetHashCode();
        public override string ToString() => GetType().Name + " [Id=" + Id + "]";

        public bool IsTransient()
        {
            if (EqualityComparer<long>.Default.Equals(Id, default))
                return true;
            
            return Convert.ToInt64(Id) <= 0;
        }
    }
}