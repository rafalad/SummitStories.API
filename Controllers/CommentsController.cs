using Microsoft.AspNetCore.Mvc;
using SummitStories.Api.Modules.Data.Interfaces;

namespace SummitStories.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet("article/{articleId}")]
        public async Task<IActionResult> GetComments(int articleId)
        {
            var comments = await _commentService.GetComments(articleId);
            return Ok(comments);
        }

        [HttpPost("article/{articleId}")]
        public async Task<IActionResult> PostComment(int articleId, [FromBody] PostCommentRequest request)
        {
            try
            {
                await _commentService.AddComment(request.Content, request.AuthorName, articleId, request.RecaptchaToken);
                return Ok();
            }
            catch (Exception ex)
            {
                // Wysyłanie pełniejszego błędu w środowisku deweloperskim
                return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
            }
        }


    }

    // Klasa żądania POST
    public class PostCommentRequest
    {
        public string Content { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
        public string RecaptchaToken { get; set; } = string.Empty;  // Nowe pole dla recaptchaToken
    }
}
