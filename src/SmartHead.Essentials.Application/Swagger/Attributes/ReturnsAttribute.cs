using System;
using Swashbuckle.AspNetCore.Annotations;

namespace SmartHead.Essentials.Application.Swagger.Attributes
{
    public class ReturnsAttribute : SwaggerResponseAttribute
    {
        public string SubStatus { get; }

        public ReturnsAttribute(int statusCode, string subStatus, string description = null, Type type = null) : base(statusCode, description, type)
        {
            SubStatus = subStatus;
        }
    }
}