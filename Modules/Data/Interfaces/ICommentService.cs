using SummitStories.Api.Modules.Data.Models;

namespace SummitStories.Api.Modules.Data.Interfaces
{
    public interface ICommentService
    {
        Task<List<Comment>> GetComments(int articleId);  // Pobieranie komentarzy
        Task AddComment(string content, string authorName, int articleId, string recaptchaToken);  // Dodawanie komentarza z recaptchaToken
    }

}
