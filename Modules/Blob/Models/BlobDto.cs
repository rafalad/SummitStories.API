namespace SummitStories.API.Modules.Blob.Models;

public class BlobDto
{
    public string Name { get; set; } = null!;
    public string ContentType { get; set; } = null!;
    public Stream? Content { get; set; }
}