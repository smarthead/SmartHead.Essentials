namespace SmartHead.Essentials.Application.Formatter
{
    public class ErrorApiResponse : ApiResponse
    {
        public ErrorApiResponse(string subStatus, object errorContent = null, string debugData = null) : base(debugData)
        {
            ErrorContent = errorContent;
            SubStatus = subStatus;
        }
        public string SubStatus { get; }
        public object ErrorContent { get; }
    }
}