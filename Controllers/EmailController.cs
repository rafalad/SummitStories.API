using Microsoft.AspNetCore.Mvc;
using SummitStories.API.Constants;
using SummitStories.API.Modules.Email.Interfaces;
using SummitStories.API.Modules.Email.Models;

namespace SummitStories.API.Controllers
{
    [Route("api")]
    public class EmailController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public EmailController(IEmailService emailService, IConfiguration configuration)
        {
            _emailService = emailService;
            _configuration = configuration;
        }

        [HttpPost("email/send")]
        public async Task<IActionResult> BusinessContactRequest([FromBody] BusinessRequestEmailDetails businessRequestDetails)
        {
            // Pobierz token reCAPTCHA z modelu
            var recaptchaToken = businessRequestDetails.RecaptchaToken;

            // Zweryfikuj reCAPTCHA
            var isRecaptchaValid = await _emailService.VerifyRecaptcha(recaptchaToken);
            if (!isRecaptchaValid)
            {
                return BadRequest("Failed reCAPTCHA verification.");
            }

            var subject = $@"Message from {businessRequestDetails.Name}";
            var AdminEmail = _configuration.GetConnectionString("MailjetApiEmail");
            var BlogOwnerEmail = _configuration.GetConnectionString("MailjetApiEmail");

            var response = await _emailService.SendEmail(EmailTemplateNames.BusinessRequest, businessRequestDetails, subject, AdminEmail, BlogOwnerEmail);

            return Ok(response);
        }
    }
}
