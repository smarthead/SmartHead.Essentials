using System;
using System.Net;

namespace SmartHead.Essentials.Application.Swagger.Attributes
{
    public class CreatedAttribute : ReturnsAttribute
    {
        public CreatedAttribute(string description = null, Type type = null) 
            : base((int)HttpStatusCode.Created, null, description, type)
        {
        }
    }
}