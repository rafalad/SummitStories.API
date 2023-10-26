using Microsoft.AspNetCore.Mvc;
using SummitStories.API.Email.Model;
using SummitStories.API.Email.HostedServices;

namespace SummitStories.API.Controllers
{
    [Route("api")]
    public class EmailController : ControllerBase
    {
        private readonly EmailHostedService _emailHostedService;

        public EmailController(EmailHostedService emailHostedService)
        {
            _emailHostedService = emailHostedService;
        }

        [HttpGet("email-confirmation-noreplay")]
        public async Task<IActionResult> TestEmail()
        {
            try
            {
                await _emailHostedService.SendEmailAsync(new EmailModel
                {
                    EmailAddress = "rafal.adamczyk01@gmail.com",
                    Subject = "Thanks!",
                    Body = "<strong>Thank you for your message. I will reply soon!<br><br>" +
                           "Cheers!<br>" +
                            "Rafał from SummitStories.blog</strong>",

                    Attachments = null
                });

                return Ok("Noreply email sent successfully");
            }
            catch (Exception)
            {
                return StatusCode(500, "Error sending noreply email");
            }
        }

        [HttpPost("email-send")]
        public async Task<IActionResult> SendEmail([FromBody] EmailModel emailRequest)
        {
            try
            {
                await _emailHostedService.SendEmailAsync(emailRequest);

                return Ok("Email sent successfully");
            }
            catch (Exception)
            {
                return StatusCode(500, "Error sending email");
            }
        }
    }
}
