using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SmartHead.Essentials.Application.Formatter;

namespace SmartHead.Essentials.Application.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    [ProducesErrorResponseType(typeof(void))]
    public abstract class FormattedApiControllerBase : ControllerBase
    {
        [NonAction]
        public BadRequestObjectResult BadRequest(string subStatus)
            => BadRequest(subStatus, null);
        [NonAction]
        public override BadRequestObjectResult BadRequest(object error)
            => BadRequest("SomethingWentWrong", error);
        [NonAction]
        public override BadRequestObjectResult BadRequest(ModelStateDictionary modelState)
            => InvalidModelStateResponseFactory.CreateFrom("InvalidModel", modelState);
        [NonAction]
        public BadRequestObjectResult BadRequest(string subStatus, object errorContent, string debugData = null)
            => new BadRequestObjectResult(new ErrorApiResponse(subStatus, errorContent, debugData));
        [NonAction]
        public OkObjectResult Ok(object content, string debugData = null)
            => new OkObjectResult(new SuccessApiResponse(content, debugData));
        [NonAction]
        public CreatedAtRouteResult CreatedAt(object routeValues, object content, string debugData = null)
            => new CreatedAtRouteResult(routeValues, new SuccessApiResponse(content, debugData));
        [NonAction]
        public UnauthorizedObjectResult Unauthorized(string error = null)
            => new UnauthorizedObjectResult(new ErrorApiResponse("Unauthorized", error));
    }
}