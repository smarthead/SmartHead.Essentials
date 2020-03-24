using System.Buffers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SmartHead.Essentials.Application.Formatter
{
    public class ResponseOutputFormatterSetup : IConfigureOptions<MvcOptions>
    {
        private readonly IWebHostEnvironment _environment;

        public ResponseOutputFormatterSetup(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        
        public void Configure(MvcOptions options)
        {
            var contractResolver = new DebugDataContractResolver(_environment)
            {
                NamingStrategy = new CamelCaseNamingStrategy(),
            };

            var jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented, 
                DateFormatString = "o"
            };
            
            options.OutputFormatters.RemoveType<SystemTextJsonOutputFormatter>();
            options.OutputFormatters.Add(new NewtonsoftJsonOutputFormatter(jsonSerializerSettings, ArrayPool<char>.Create(), new MvcOptions()));
        }
    }
}