using System;
using System.Net;

namespace SmartHead.Essentials.Application.Swagger.Attributes
{
    public class OkAttribute : ReturnsAttribute
    {
        public OkAttribute(string description = null, Type type = null) 
            : base((int)HttpStatusCode.OK, null, description, type)
        {
        }
    }
}