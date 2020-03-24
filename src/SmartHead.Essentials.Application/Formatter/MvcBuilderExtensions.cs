using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace SmartHead.Essentials.Application.Formatter
{
    public static class MvcBuilderExtensions
    {
        public static IMvcBuilder AddResponseOutputFormatter(this IMvcBuilder builder)
        {
            builder
                .Services
                .TryAddEnumerable(ServiceDescriptor
                    .Transient<IConfigureOptions<MvcOptions>, ResponseOutputFormatterSetup>());
            
            return builder;
        }
    }
}