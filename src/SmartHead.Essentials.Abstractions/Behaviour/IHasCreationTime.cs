using System;
using System.ComponentModel.DataAnnotations;

namespace SmartHead.Essentials.Abstractions.Behaviour
{
    public interface IHasCreationTime
    {
        [Required]
        DateTime CreationTime { get; set; }
    }
}