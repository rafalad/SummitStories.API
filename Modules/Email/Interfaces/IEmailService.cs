namespace SummitStories.API.Modules.Email.Interfaces
{
    public interface IEmailService
    {
        Task<string> SendEmail(string templateName, dynamic templateModel, string subject, string sender, string recipient);
        Task<string> SendConfirmationEmail(dynamic templateModel, string sender, string recipient);
        Task<bool> VerifyRecaptcha(string recaptchaToken);
    }
}
