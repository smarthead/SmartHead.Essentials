using System;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.OpenApi.Models;
using SmartHead.Essentials.Application.Controller;
using SmartHead.Essentials.Application.Swagger.Attributes;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SmartHead.Essentials.Application.Swagger
{
    public class BadRequestOperationFilter<T> : IOperationFilter
        where T: IDescriptionProvider
    {
        private IDescriptionProvider _descriptionProvider = null;

        public IDescriptionProvider Description =>
            _descriptionProvider ??= Activator.CreateInstance<T>();
            
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var isController = context.MethodInfo.ReflectedType?.BaseType == typeof(FormattedApiControllerBase) ||
                               context.MethodInfo.ReflectedType?.BaseType?.BaseType == typeof(FormattedApiControllerBase) ||
                               context.MethodInfo.ReflectedType?.BaseType?.BaseType?.BaseType == typeof(FormattedApiControllerBase);
            if (!isController) return;
            
            var groups = context.MethodInfo.DeclaringType
                .GetCustomAttributes(true)
                .Union(context.MethodInfo.GetCustomAttributes(true))
                .OfType<ReturnsAttribute>()
                .Where(x => x.StatusCode == (int)HttpStatusCode.BadRequest)
                .GroupBy(x => x.StatusCode);

            foreach (var group in groups)
            {
                var count = group.Count();
                var responses = group.ToArray();
                
                for (var i = 0; i < count; i++)
                {
                    var response = responses[i];
                    
                    var key = $"{response.StatusCode}";
                    if (count > 1)
                        key += $" ({i + 1} из {count})";
                    
                    if (operation.Responses.ContainsKey(response.StatusCode.ToString()))
                        operation.Responses.Remove(response.StatusCode.ToString());

                    if (operation.Responses.ContainsKey(key)) 
                        continue;
                    
                    var apiResponseType = context
                        .ApiDescription
                        .SupportedResponseTypes
                        .FirstOrDefault(x => x.Type == response.Type);

                    operation.Responses.Add(key, CreateOpenApiResponse(context, response, apiResponseType));
                }
            }
        }

        private OpenApiResponse CreateOpenApiResponse(OperationFilterContext context, SwaggerResponseAttribute response, ApiResponseType apiResponseType) 
            => new OpenApiResponse()
            {
                Description = Description.GetValue(response.Description),
                Content = apiResponseType
                    ?.ApiResponseFormats
                    .ToDictionary(x => x.MediaType,
                        x => CreateResponseMediaType(context, apiResponseType.ModelMetadata))
            };

        private OpenApiMediaType CreateResponseMediaType(OperationFilterContext context, ModelMetadata modelMetadata) 
            => new OpenApiMediaType
            {
                Schema = context.SchemaGenerator.GenerateSchema(modelMetadata.ModelType, context.SchemaRepository)
            };
    }
}