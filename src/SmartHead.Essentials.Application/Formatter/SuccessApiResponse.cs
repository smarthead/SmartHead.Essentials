namespace SmartHead.Essentials.Application.Formatter
{
    public class SuccessApiResponse : ApiResponse
    {
        public SuccessApiResponse(object content, string debugData = null) : base(debugData)
        {
            Content = content;
        }

        public object Content { get; set; }
    }
}