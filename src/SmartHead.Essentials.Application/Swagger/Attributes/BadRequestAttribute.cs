using System;
using System.Net;

namespace SmartHead.Essentials.Application.Swagger.Attributes
{
    public class BadRequestAttribute : ReturnsAttribute
    {
        public BadRequestAttribute(string subStatus, string description = null, Type type = null) 
            :base((int)HttpStatusCode.BadRequest, subStatus, description, type)
        {
        }
    }
}