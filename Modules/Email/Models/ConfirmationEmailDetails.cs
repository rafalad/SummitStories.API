namespace SummitStories.API.Modules.Email.Models
{
    public class ConfirmationEmailDetails
    {
        public required string Name { get; set; }

        public required string Email { get; set; }

        public required string Message { get; set; }
    }
}