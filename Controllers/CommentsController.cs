using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SummitStories.API.Modules.Data.Models;
using SummitStories.API.Modules.Data.Interfaces;
using System.Reflection.Metadata;

namespace SummitStories.API.Controllers;

[ApiController]
[Route("api")]
[Produces("application/json")]
public class CommentsController : ControllerBase
{
    private readonly ICommentRepository _commentRepository;

    public CommentsController(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    [HttpGet("articles/{articleId}/comments")]
    public IActionResult GetCommentsForArticle([FromRoute] int articleId)
    {
        IList<Comment> results = _commentRepository.GetCommentsForArticle(articleId);
        return Ok(results);
    }

    [HttpPost("articles/{articleId}/comments")]
    public async Task<IActionResult> CreateCommentForArticle([FromRoute] int articleId, [FromBody] Comment comment)
    {
        try
        {
            var createdComment = await _commentRepository.CreateCommentForArticleAsync(articleId, comment);
            return CreatedAtAction("GetComment", new { id = createdComment.CommentId }, createdComment);
        }
        catch (InvalidOperationException ex)
        {
            // Obsłuż przypadek, gdy artykuł nie istnieje.
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            // Obsłuż inne błędy.
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }


    [HttpGet("articles/{articleId}/comments/{commentId}")]
    public async Task<IActionResult> GetCommentForArticle([FromRoute] int articleId, [FromRoute] int commentId)
    {
        try
        {
            var comment = await _commentRepository.GetCommentForArticle(articleId, commentId);

            if (comment == null)
            {
                return NotFound(); // Komentarz nie został znaleziony.
            }

            return Ok(comment);
        }
        catch (Exception ex)
        {
            // Obsłuż błąd, np. zaloguj go lub zwróć odpowiedź błędu.
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }


    [HttpPut("articles/{articleId}/comments/{commentId}")]
    public async Task<IActionResult> UpdateCommentForArticle([FromRoute] int articleId, [FromRoute] int commentId, [FromBody] Comment updatedComment)
    {
        try
        {
            // Upewnij się, że komentarz o identyfikatorze `commentId` istnieje i należy do artykułu o identyfikatorze `articleId`.
            var existingComment = await _commentRepository.GetCommentForArticle(articleId, commentId);

            if (existingComment == null)
            {
                return NotFound(); // Komentarz nie został znaleziony.
            }

            // Aktualizuj pola komentarza na podstawie wartości zawartych w `updatedComment`.
            existingComment.Content = updatedComment.Content;
            existingComment.Author = updatedComment.Author;

            // Wywołaj metodę repozytorium do aktualizacji komentarza.
            await _commentRepository.UpdateComment(existingComment);

            return Ok(existingComment);
        }
        catch (Exception ex)
        {
            // Obsłuż błąd, np. zaloguj go lub zwróć odpowiedź błędu.
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpDelete("articles/{articleId}/comments/{commentId}")]
    public async Task<IActionResult> DeleteCommentForArticle([FromRoute] int articleId, [FromRoute] int commentId)
    {
        try
        {
            // Upewnij się, że komentarz o identyfikatorze `commentId` istnieje i należy do artykułu o identyfikatorze `articleId`.
            var existingComment = await _commentRepository.GetCommentForArticle(articleId, commentId);

            if (existingComment == null)
            {
                return NotFound(); // Komentarz nie został znaleziony.
            }

            // Usuń komentarz.
            await _commentRepository.DeleteCommentForArticle(articleId, commentId);

            return NoContent(); // Komentarz został pomyślnie usunięty.
        }
        catch (Exception ex)
        {
            // Obsłuż błąd, np. zaloguj go lub zwróć odpowiedź błędu.
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}

