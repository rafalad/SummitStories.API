namespace SummitStories.API.Modules.Email.Models
{
    public class BusinessRequestEmailDetails
    {
        public required string Name { get; set; }

        public required string Email { get; set; }

        public required string ApplicationOwnerEmail { get; set; }

        public required string Message { get; set; }
    }
}