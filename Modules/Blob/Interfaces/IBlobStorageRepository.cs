using SummitStories.API.AzureBlobStorage;
using SummitStories.API.Modules.Blob.Models;

namespace SummitStories.API.Modules.Blob.Interfaces;

public interface IBlobStorageRepository
{
    Task<BlobDto?> GetFileAsync(string containerName, string filePath);
}
