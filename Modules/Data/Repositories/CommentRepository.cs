using Microsoft.EntityFrameworkCore;
using SummitStories.Api.Modules.Data.Interfaces;
using SummitStories.Api.Modules.Data.Models;

namespace SummitStories.Api.Modules.Data.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;

        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Comment>> GetCommentsByArticleId(int articleId)
        {
            return await _context.Comments.Where(c => c.ArticleId == articleId).ToListAsync();
        }

        public async Task AddComment(Comment comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
        }
    }

}
