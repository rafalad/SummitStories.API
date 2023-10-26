using Azure.Storage.Blobs.Models;
using Azure;

namespace SummitStories.API.Modules.Azure.Interfaces;

public interface IAzureStorageService
{
    public Task<Response<BlobDownloadStreamingResult>?> DownloadStreamingAsync(string containerName, string blockBlobReference);
}
