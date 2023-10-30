namespace SummitStories.API.Modules.Data.Models;

public partial class Article
{
    public int ArticleId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedDate { get; set; }
    public ICollection<Comment> Comments { get; set; }
}
