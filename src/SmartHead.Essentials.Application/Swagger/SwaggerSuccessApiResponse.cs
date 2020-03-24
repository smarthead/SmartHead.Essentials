using SmartHead.Essentials.Application.Formatter;

namespace SmartHead.Essentials.Application.Swagger
{
    public class SwaggerSuccessApiResponse<T> : ApiResponse
    {
        public SwaggerSuccessApiResponse(T content, string debugData = null) : base(debugData)
        {
            Content = content;
        }

        public T Content { get; set; }
    }
}