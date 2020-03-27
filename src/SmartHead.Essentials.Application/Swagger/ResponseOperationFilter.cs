using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using SmartHead.Essentials.Application.Controller;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SmartHead.Essentials.Application.Swagger
{
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    public class ResponseOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var isController = context.MethodInfo.ReflectedType.BaseType == typeof(FormattedApiControllerBase) ||
                               context.MethodInfo.ReflectedType.BaseType.BaseType == typeof(FormattedApiControllerBase) ||
                               context.MethodInfo.ReflectedType.BaseType.BaseType.BaseType == typeof(FormattedApiControllerBase);
            if (!isController) return;

            if (!operation.Responses.ContainsKey("500"))
                operation.Responses.Add("500", new OpenApiResponse {Description = SwaggerResponseMessages.ServerError});

            var hasAuthAttribute = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
                .Union(context.MethodInfo.GetCustomAttributes(true))
                .OfType<AuthorizeAttribute>()
                .Any();

            if (hasAuthAttribute && !operation.Responses.ContainsKey("401"))
                operation.Responses.Add("401", new OpenApiResponse {Description = SwaggerResponseMessages.Unauthorized});

            if (hasAuthAttribute && !operation.Responses.ContainsKey("403"))
                operation.Responses.Add("403", new OpenApiResponse {Description = SwaggerResponseMessages.Forbidden});
        }
    }
}