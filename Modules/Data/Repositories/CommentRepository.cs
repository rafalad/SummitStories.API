using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using SummitStories.API.Modules.Data.Interfaces;
using SummitStories.API.Modules.Data.Models;
using SummitStories.API.Modules.SqlDb.Interfaces;
using System.Data;

namespace SummitStories.API.Modules.Data.Repositories;

public class CommentRepository : GenericRepository, ICommentRepository
{
    public CommentRepository(ISqlDbService sqlDbService, ILogger<CommentRepository> logger) : base(sqlDbService, logger) { }

    public IList<Comment> GetCommentsForArticle(int articleId)
    {
        IList<Comment> comments = new List<Comment>();
        IList<IDbDataParameter> paramValues = new List<IDbDataParameter>
        {
            new SqlParameter("@articleId", articleId)
        };
        string queryString =
            "SELECT * FROM dbo.Comments WHERE ArticleId = @articleId";


        IList<IReadOnlyDictionary<string, dynamic>> results = Read(queryString, paramValues);
        _logger.LogTrace("Raw result: {RawResult}", JsonConvert.SerializeObject(results));

        foreach (IReadOnlyDictionary<string, dynamic> result in results)
        {
            comments.Add(new Comment
            {
                CommentId = result.GetValueOrDefault("CommentId", ""),
                Content = result.GetValueOrDefault("Content", ""),
                Author = result.GetValueOrDefault("Author", ""),
                CreatedDate = result.GetValueOrDefault("CreatedDate", "")
            });
        }
        _logger.LogTrace("Result: {Result}", JsonConvert.SerializeObject(comments));

        return comments;
    }

    public async Task<Comment> CreateCommentForArticleAsync(int articleId, Comment comment)
    {
        if (comment == null)
        {
            throw new ArgumentNullException(nameof(comment));
        }

        string insertQuery = "INSERT INTO Comments (ArticleId, Content, Author, CreatedDate) " +
                             "VALUES (@articleId, @content, @author, @createdDate); SELECT SCOPE_IDENTITY();";

        IList<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@articleId", articleId),
                new SqlParameter("@content", comment.Content),
                new SqlParameter("@author", comment.Author),
                new SqlParameter("@createdDate", DateTime.Now)
            };

        try
        {
            // Wykonaj operację wstawienia i pobierz ID nowo utworzonego komentarza.
            object commentIdObj = await ExecuteScalarAsync(insertQuery, parameters);

            if (commentIdObj != null && int.TryParse(commentIdObj.ToString(), out int commentId))
            {
                comment.CommentId = commentId;
                return comment;
            }
            else
            {
                throw new Exception("Nie udało się utworzyć komentarza.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating a comment.");
            throw;
        }
    }

    public async Task<Comment> GetCommentForArticle(int articleId, int commentId)
    {
        Comment comment = null;
        IList<IDbDataParameter> paramValues = new List<IDbDataParameter>
    {
        new SqlParameter("@articleId", articleId),
        new SqlParameter("@commentId", commentId)
    };
        string queryString =
            "SELECT * FROM dbo.Comments WHERE ArticleId = @articleId AND CommentId = @commentId";

        IList<IReadOnlyDictionary<string, dynamic>> results = Read(queryString, paramValues);
        _logger.LogTrace("Raw result: {RawResult}", JsonConvert.SerializeObject(results));

        if (results.Count > 0)
        {
            var result = results.First();
            comment = new Comment
            {
                CommentId = result.GetValueOrDefault("CommentId", ""),
                Content = result.GetValueOrDefault("Content", ""),
                Author = result.GetValueOrDefault("Author", ""),
                CreatedDate = result.GetValueOrDefault("CreatedDate", "")
            };
        }
        _logger.LogTrace("Result: {Result}", JsonConvert.SerializeObject(comment));

        return comment;
    }

    public async Task<Comment> UpdateCommentForArticle(int articleId, int commentId, Comment updatedComment)
    {
        // Upewnij się, że komentarz o identyfikatorze `commentId` istnieje i należy do artykułu o identyfikatorze `articleId`.

        Comment existingComment = await GetCommentForArticle(articleId, commentId);

        if (existingComment == null)
        {
            return null; // Komentarz nie został znaleziony.
        }

        // Tutaj możesz przeprowadzić logikę aktualizacji komentarza na podstawie wartości zawartych w `updatedComment`.

        existingComment.Content = updatedComment.Content;
        existingComment.Author = updatedComment.Author;

        // Aktualizuj komentarz w bazie danych.
        string updateQuery = @"
        UPDATE dbo.Comments
        SET Content = @Content, Author = @Author
        WHERE ArticleId = @ArticleId AND CommentId = @CommentId";

        var parameters = new List<SqlParameter>
    {
        new SqlParameter("@ArticleId", articleId),
        new SqlParameter("@CommentId", commentId),
        new SqlParameter("@Content", existingComment.Content),
        new SqlParameter("@Author", existingComment.Author)
    };

        await ExecuteNonQueryAsync(updateQuery, parameters);

        return existingComment;
    }

    public async Task UpdateComment(Comment comment)
    {
        string queryString =
            "UPDATE dbo.Comments " +
            "SET Content = @Content, Author = @Author " +
            "WHERE CommentId = @CommentId";

        var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Content", comment.Content),
                new SqlParameter("@Author", comment.Author),
                new SqlParameter("@CommentId", comment.CommentId)
            };

        await ExecuteNonQueryAsync(queryString, parameters);
    }

    public async Task DeleteCommentForArticle(int articleId, int commentId)
    {
        // Upewnij się, że komentarz o identyfikatorze `commentId` istnieje i należy do artykułu o identyfikatorze `articleId`.
        var existingComment = await GetCommentForArticle(articleId, commentId);

        if (existingComment == null)
        {
            return; // Komentarz nie został znaleziony, więc nie można go usunąć.
        }

        // Usuń komentarz z bazy danych.
        string deleteQuery = @"
                DELETE FROM dbo.Comments
                WHERE ArticleId = @ArticleId AND CommentId = @CommentId";

        var parameters = new List<SqlParameter>
            {
                new SqlParameter("@ArticleId", articleId),
                new SqlParameter("@CommentId", commentId)
            };

        await ExecuteNonQueryAsync(deleteQuery, parameters);
    }
}