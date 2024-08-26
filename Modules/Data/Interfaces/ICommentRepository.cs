using SummitStories.Api.Modules.Data.Models;

namespace SummitStories.Api.Modules.Data.Interfaces
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetCommentsByArticleId(int articleId);
        Task AddComment(Comment comment);
    }

}
