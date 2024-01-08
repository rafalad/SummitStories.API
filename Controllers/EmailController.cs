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
            var subject = $@"Inquiry from {businessRequestDetails.Name}""";
            var LoefiAdminEmail = _configuration.GetValue<string>(nameof(AzureKeyVaultConfig.MailjetApiEmail)) ?? "";

            var response = await _emailService.SendEmail(EmailTemplateNames.BusinessRequest, businessRequestDetails, subject, LoefiAdminEmail, businessRequestDetails.ApplicationOwnerEmail);

            return Ok(response);
        }
    }
}
