using System;

namespace SmartHead.Essentials.Abstractions.Behaviour
{
    public interface IHasModificationTime
    {
        DateTime? LastModificationTime { get; set; }
    }
}