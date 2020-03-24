using SmartHead.Essentials.Application.Formatter;

namespace SmartHead.Essentials.Application.Swagger
{
    public class SwaggerErrorApiResponse<T> : ApiResponse
    {
        public SwaggerErrorApiResponse(string subStatus, T errorContent = default, string debugData = null) : base(debugData)
        {
            ErrorContent = errorContent;
            SubStatus = subStatus;
        }

        public string SubStatus { get; }
        public T ErrorContent { get; }
    }
}
