using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SmartHead.Essentials.Abstractions.Behaviour;

namespace SmartHead.Essentials.Implementation.Events
{
    public class UserContext : IUser
    {
        protected readonly IHttpContextAccessor Accessor;
        public UserContext(IHttpContextAccessor accessor) 
            => Accessor = accessor;

        public virtual string Name 
            => Accessor.HttpContext.User.Identity.Name;

        public virtual bool IsAuthenticated() 
            => Accessor.HttpContext.User.Identity.IsAuthenticated;

        public virtual IEnumerable<Claim> Claims() 
            => Accessor.HttpContext.User.Claims;
    }
}