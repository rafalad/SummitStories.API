using Azure.Storage.Blobs.Models;
using Azure;
using SummitStories.API.Modules.Azure.Interfaces;
using SummitStories.API.Modules.Azure.Services;
using SummitStories.API.Modules.Blob.Models;
using SummitStories.API.Modules.Blob.Interfaces;
using SummitStories.API.Models;

namespace SummitStories.API.Modules.Blob.Repositories;

public class BlobStorageRepository : IBlobStorageRepository
{
    private readonly IAzureStorageService _azureStorageService;
    private readonly ILogger<BlobStorageRepository> _logger;

    public BlobStorageRepository(ILogger<BlobStorageRepository> logger, IAzureStorageService? service = null, string? connectionString = null)
    {
        _logger = logger;
        _logger.LogTrace("Connecting to Blob Storage using connection string: {ConnectionString}", connectionString);
        _azureStorageService = service ?? new AzureStorageService(null, connectionString);
    }

    public async Task<BlobDto?> GetFileAsync(string containerName, string filePath)
    {
        try
        {
            _logger.LogTrace("Getting Blob content: {ContainerName}/{FilePath}", containerName, filePath);
            Response<BlobDownloadStreamingResult>? content = await _azureStorageService.DownloadStreamingAsync(containerName, filePath);
            _logger.LogTrace("Blob content details: {Details}", content?.Value.Details);

            return new BlobDto
            {
                Content = content?.Value.Content,
                Name = filePath.Split("/", StringSplitOptions.RemoveEmptyEntries).LastOrDefault(""),
                ContentType = content?.Value.Details?.ContentType ?? "application/octet-stream"
            };
        }
        catch (RequestFailedException ex)
        {
            if (ex.ErrorCode == BlobErrorCode.BlobNotFound)
            {
                throw new ApiException(
                    StatusCodes.Status404NotFound,
                    $"File {containerName}/{filePath} was not found.",
                    ex.Message);
            }
            else
            {
                throw;
            }
        }
    }
}
