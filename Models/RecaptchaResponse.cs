namespace SummitStories.Api.Models
{
    public class RecaptchaResponse
    {
        public bool Success { get; set; }
        public string ChallengeTs { get; set; } = string.Empty;  // Timestamp
        public string Hostname { get; set; } = string.Empty;  // Hostname of the site where the reCAPTCHA was solved
        public List<string> ErrorCodes { get; set; } = new List<string>();  // List of error codes, if any
    }
}
