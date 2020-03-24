using System;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SmartHead.Essentials.Application.Formatter
{
    public class DebugDataContractResolver : DefaultContractResolver
    {
        private readonly IWebHostEnvironment _environment;

        public DebugDataContractResolver(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            if (property.PropertyName.Equals(nameof(ErrorApiResponse.DebugData), StringComparison.InvariantCultureIgnoreCase))
            {
                property.ShouldSerialize =
                    instance => _environment.IsDevelopment();
            }

            return property;
        }
    }
}