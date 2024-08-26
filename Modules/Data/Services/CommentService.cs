using Newtonsoft.Json;
using SummitStories.Api.Models;
using SummitStories.Api.Modules.Data.Interfaces;
using SummitStories.Api.Modules.Data.Models;

namespace SummitStories.Api.Modules.Data.Services;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public CommentService(ICommentRepository commentRepository, IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _commentRepository = commentRepository ?? throw new ArgumentNullException(nameof(commentRepository));
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    // Implementacja metody pobierania komentarzy
    public async Task<List<Comment>> GetComments(int articleId)
    {
        return await _commentRepository.GetCommentsByArticleId(articleId);
    }

    // Metoda weryfikacji reCAPTCHA
    public async Task<bool> VerifyRecaptcha(string recaptchaToken)
    {
        try
        {
            var secretKey = _configuration["GoogleRecaptcha:SecretKey"];
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new Exception("Google reCAPTCHA secret key is not configured.");
            }

            var client = _httpClientFactory.CreateClient();

            var response = await client.PostAsync($"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={recaptchaToken}", null);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"reCAPTCHA verification failed: {response.StatusCode}");
                return false;
            }

            var result = await response.Content.ReadAsStringAsync();
            var recaptchaResponse = JsonConvert.DeserializeObject<RecaptchaResponse>(result);

            if (recaptchaResponse == null || !recaptchaResponse.Success)
            {
                Console.WriteLine("reCAPTCHA response was null or verification failed.");
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error verifying reCAPTCHA: {ex.Message}");
            return false;
        }
    }

    // Metoda dodawania komentarza z obsługą reCAPTCHA
    public async Task AddComment(string content, string authorName, int articleId, string recaptchaToken)
    {
        if (!await VerifyRecaptcha(recaptchaToken))
        {
            throw new Exception("Failed reCAPTCHA verification");
        }

        var comment = new Comment
        {
            Content = content,
            AuthorName = authorName,
            ArticleId = articleId,
            CreatedAt = DateTime.UtcNow
        };

        if (_commentRepository == null) throw new Exception("Comment repository is not initialized.");
        await _commentRepository.AddComment(comment);
    }
}
