using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;

namespace SmartHead.Essentials.Application.Attributes
{
    public class DevelopmentOnlyAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            var env = (IWebHostEnvironment) context.HttpContext.RequestServices.GetService(typeof(IWebHostEnvironment));
            if (!env.IsDevelopment())
            {
                context.Result = new NotFoundObjectResult(default);
                return;
            }

            await next();
        }
    }
}