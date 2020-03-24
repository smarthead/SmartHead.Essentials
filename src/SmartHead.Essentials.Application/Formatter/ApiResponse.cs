namespace SmartHead.Essentials.Application.Formatter
{
    public abstract class ApiResponse
    {
        protected ApiResponse(string debugData = null)
        {
            DebugData = debugData;
        }

        public string DebugData { get; set; }
    }
}