using Mailjet.Client;
using SummitStories.API.Constants;
using SummitStories.API.Email.Interfaces;
using SummitStories.API.Email.Model;

namespace SummitStories.API.Email.EmailSender
{
    public abstract class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public MailjetClient CreateMailJetClient()
        {
            string mailjetApiKey = _configuration.GetValue<string>(nameof(AzureKeyVaultConfig.MailjetApiKey)) ?? "";
            string mailjetApiSecret = _configuration.GetValue<string>(nameof(AzureKeyVaultConfig.MailjetApiSecret)) ?? "";

            return new MailjetClient(mailjetApiKey, mailjetApiSecret);
        }

        protected abstract Task Send(EmailModel email);

        public async Task SendEmail(EmailModel emailModel)
        {
            try
            {
                await Send(emailModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Sending email failed: {ex.Message}");
                throw;
            }
        }

        public async Task SendEmail(string address, string subject, string body, List<EmailAttachment>? emailAttachment = null)
        {
            await Send(new EmailModel
            {
                Attachments = emailAttachment!,
                Body = body,
                EmailAddress = address,
                Subject = subject,
            });
        }
    }
}
