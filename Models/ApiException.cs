namespace SummitStories.API.Models
{
    public class ApiException : Exception
    {
        public int StatusCode { get; set; }
        public override string Message { get; } = null!;
        public string? Content { get; set; }

        public ApiException(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }

        public ApiException(int statusCode, string message, string? content = null)
        {
            StatusCode = statusCode;
            Content = content;
            Message = message;
        }
    }
}
