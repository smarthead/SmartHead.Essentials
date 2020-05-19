using System;
using System.Net;

namespace SmartHead.Essentials.Application.Swagger.Attributes
{
    public class NoContentAttribute : ReturnsAttribute
    {
        public NoContentAttribute(string description = null) 
            : base((int)HttpStatusCode.NoContent, null, description, null)
        {
        }
    }
}