using System;
using System.Net;

namespace SmartHead.Essentials.Application.Swagger.Attributes
{
    public class InternalServerErrorAttribute : ReturnsAttribute
    {
        public InternalServerErrorAttribute(string subStatus = null, string description = null, Type type = null) 
            : base((int)HttpStatusCode.InternalServerError, subStatus, description, type)
        {
        }
    }
}