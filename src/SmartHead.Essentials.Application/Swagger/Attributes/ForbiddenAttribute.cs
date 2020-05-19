using System;
using System.Net;

namespace SmartHead.Essentials.Application.Swagger.Attributes
{
    public class ForbiddenAttribute : ReturnsAttribute
    {
        public ForbiddenAttribute(string subStatus = null, string description = null, Type type = null) 
            : base((int)HttpStatusCode.Forbidden, subStatus, description, type)
        {
        }
    }
}