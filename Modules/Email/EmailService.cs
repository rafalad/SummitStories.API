using Mailjet.Client;
using Mailjet.Client.TransactionalEmails;
using SummitStories.API.Modules.Email.Interfaces;

namespace SummitStories.API.Modules.Email
{
    public class EmailService : IEmailService
    {
        private readonly IMailjetClient _mailjetClient;
        private readonly IRazorViewToStringRenderer _razorViewToStringRenderer;

        public EmailService(IMailjetClient mailjetClient, IRazorViewToStringRenderer razorViewToStringRenderer)
        {
            _mailjetClient = mailjetClient;
            _razorViewToStringRenderer = razorViewToStringRenderer;
        }

        public async Task<string> SendEmail(string templateName, dynamic templateModel, string subject, string sender, string recipient)
        {
            var templatePath = $"/Modules/Email/Templates/{templateName}.cshtml";
            string body = _razorViewToStringRenderer.RenderViewToStringAsync(templatePath, templateModel).Result;

            var email = new TransactionalEmailBuilder()
                   .WithFrom(new SendContact(sender))
                   .WithSubject(subject)
                   .WithHtmlPart(body)
                   .WithTo(new SendContact(recipient))
                   .Build();

            var response = await _mailjetClient.SendTransactionalEmailAsync(email);
            return response.Messages[0].Status.ToString();
        }
    }
}
