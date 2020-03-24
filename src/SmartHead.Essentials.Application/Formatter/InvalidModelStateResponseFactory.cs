using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SmartHead.Essentials.Application.Formatter
{
    public static class InvalidModelStateResponseFactory
    {
        public static BadRequestObjectResult CreateFrom(string subStatus, ModelStateDictionary modelState)
        {
            var errors = modelState
                .Where(x => x.Value.Errors.Count > 0)
                .ToDictionary(
                    x => x.Key,
                    x => x.Value
                        .Errors
                        .Select(e => e.ErrorMessage)
                        .ToArray()
                );
            
            return new BadRequestObjectResult(new ErrorApiResponse(subStatus, errors));
        }
    }
}