namespace WorkMosmApi.Models.Errors
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }
        public string TraceId { get; set; }
    }
}
