using SummitStories.API.Modules.Data.Models;

namespace SummitStories.API.Modules.Data.Interfaces;

public interface ICommentRepository : IGenericRepository
{
    public IList<Comment> GetCommentsForArticle(int articleId);
    Task<Comment> GetCommentForArticle(int articleId, int commentId);
    Task<Comment> CreateCommentForArticleAsync(int articleId, Comment comment);
    Task<Comment> UpdateCommentForArticle(int articleId, int commentId, Comment updatedComment);
    Task UpdateComment(Comment comment);
    Task DeleteCommentForArticle(int articleId, int commentId);
}