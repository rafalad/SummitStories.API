namespace SummitStories.API.Modules.Data.Models;

public class Comment
{
    public int CommentId { get; set; }
    public string Content { get; set; }
    public string Author { get; set; }
    public DateTime CreatedDate { get; set; }
}
