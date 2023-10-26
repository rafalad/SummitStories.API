namespace SummitStories.API.Models
{
    public class ErrorResponse
    {
        public required int StatusCode { get; set; }
        public required string Error { get; set; }
        public required string Message { get; set; }
        public object? AdditionalInfo { get; set; }
    }
}
