namespace SummitStories.Api.Modules.Data.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Relacja do artykułu
        public int ArticleId { get; set; }
        public Article Article { get; set; } = default!;
    }

}
