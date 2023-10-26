using SummitStories.API.Email.Model;

namespace SummitStories.API.Email.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmail(string address, string subject, string body, List<EmailAttachment>? emailAttachment = null);
        Task SendEmail(EmailModel emailModel);
    }
}
