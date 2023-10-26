using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Azure;
using SummitStories.API.Modules.Azure.Interfaces;

namespace SummitStories.API.Modules.Azure.Services;

public class AzureStorageService : IAzureStorageService
{
    private readonly BlobServiceClient _blobServiceClient;

    public AzureStorageService(BlobServiceClient? client = null, string? connectionString = null)
    {
        _blobServiceClient = client ?? new(connectionString ?? "");
    }

    public async Task<Response<BlobDownloadStreamingResult>?> DownloadStreamingAsync(string containerName, string blockBlobReference)
    {
        BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        BlobClient blob = blobContainerClient.GetBlobClient(blockBlobReference);
        return await blob.DownloadStreamingAsync();
    }
}
