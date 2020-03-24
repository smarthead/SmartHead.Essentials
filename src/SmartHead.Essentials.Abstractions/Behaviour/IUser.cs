using System.Collections.Generic;
using System.Security.Claims;

namespace SmartHead.Essentials.Abstractions.Behaviour
{
    public interface IUser
    {
        string Name { get; }
        bool IsAuthenticated();
        IEnumerable<Claim> Claims();
    }
}