using Mailjet.Client;
using Mailjet.Client.TransactionalEmails;
using Newtonsoft.Json;
using SummitStories.Api.Models;
using SummitStories.API.Modules.Email.Interfaces;
using System.Net.Http;

namespace SummitStories.API.Modules.Email
{
    public class EmailService : IEmailService
    {
        private readonly IMailjetClient _mailjetClient;
        private readonly IRazorViewToStringRenderer _razorViewToStringRenderer;
        private readonly ILogger<EmailService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public EmailService(
            IMailjetClient mailjetClient,
            IRazorViewToStringRenderer razorViewToStringRenderer,
            ILogger<EmailService> logger,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            _mailjetClient = mailjetClient;
            _razorViewToStringRenderer = razorViewToStringRenderer;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<bool> VerifyRecaptcha(string recaptchaToken)
        {
            try
            {
                var secretKey = _configuration["GoogleRecaptcha:SecretKey"];
                if (string.IsNullOrEmpty(secretKey))
                {
                    throw new Exception("Google reCAPTCHA secret key is not configured.");
                }

                var client = _httpClientFactory.CreateClient();

                var response = await client.PostAsync($"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={recaptchaToken}", null);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"reCAPTCHA verification failed: {response.StatusCode}");
                    return false;
                }

                var result = await response.Content.ReadAsStringAsync();
                var recaptchaResponse = JsonConvert.DeserializeObject<RecaptchaResponse>(result);

                if (recaptchaResponse == null || !recaptchaResponse.Success)
                {
                    Console.WriteLine("reCAPTCHA response was null or verification failed.");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error verifying reCAPTCHA: {ex.Message}");
                return false;
            }
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

            if (response.Messages[0].Status.ToLower() == "success")
            {
                _logger.LogInformation("Email sent successfully to {Recipient}.", recipient);

                // Wydobywanie adresu email z modelu templateModel
                string recipientEmail = templateModel.Email;

                // Wywołanie metody SendConfirmationEmail z wykorzystaniem e-maila odbiorcy z modelu danych
                var confirmationStatus = await SendConfirmationEmail(templateModel, sender, recipientEmail);

                if (confirmationStatus.ToLower() == "success")
                {
                    _logger.LogInformation("Confirmation email sent successfully to {Recipient}.", recipientEmail);
                    return "Both email and confirmation email were sent successfully.";
                }
                else
                {
                    _logger.LogError("Failed to send confirmation email to {Recipient}.", recipientEmail);
                    return "Email sent successfully, but failed to send confirmation email.";
                }
            }
            else
            {
                _logger.LogError("Failed to send email to {Recipient}.", recipient);
                return "Failed to send email.";
            }
        }

        public async Task<string> SendConfirmationEmail(dynamic templateModel, string sender, string recipient)
        {
            var templatePath = $"/Modules/Email/Templates/ConfirmationTemplate.cshtml";
            var subjectConfirmation = "Message from SummitStories.blog";
            string body = _razorViewToStringRenderer.RenderViewToStringAsync(templatePath, templateModel).Result;

            var email = new TransactionalEmailBuilder()
                   .WithFrom(new SendContact(sender))
                   .WithSubject(subjectConfirmation)
                   .WithHtmlPart(body)
                   .WithTo(new SendContact(recipient))
                   .Build();

            var response = await _mailjetClient.SendTransactionalEmailAsync(email);
            return response.Messages[0].Status.ToString();
        }
    }
}
