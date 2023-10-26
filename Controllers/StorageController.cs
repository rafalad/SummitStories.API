using Microsoft.AspNetCore.Mvc;
using SummitStories.API.Models;
using SummitStories.API.Modules.Blob.Interfaces;
using SummitStories.API.Modules.Blob.Models;

namespace SummitStories.API.AzureBlobStorage;

[Route("api")]
[ApiController]
public class StorageController : ControllerBase
{
    private readonly IBlobStorageRepository _storage;

    public StorageController(IBlobStorageRepository storage)
    {
        _storage = storage;
    }

    [HttpGet("file/{*filePath}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileStreamResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiException))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiException))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiException))]
    public async Task<IActionResult> GetFile([FromRoute] string filePath)
    {
        int filePathCount = 2;
        string[] filePathTokens = filePath.Split("/", filePathCount, StringSplitOptions.RemoveEmptyEntries);
        if (filePathTokens.Length < filePathCount)
        {
            throw new ApiException(StatusCodes.Status400BadRequest, "Invalid inputs");
        }

        string containerName = filePathTokens[0];
        string processedFilePath = filePathTokens[1];
        BlobDto? file = await _storage.GetFileAsync(containerName, processedFilePath);
        if (file == null || file.Content == null)
        {
            throw new ApiException(StatusCodes.Status404NotFound, $"The specified blob '{filePath}' does not exist.");
        }

        return File(fileStream: file.Content,
            contentType: file.ContentType,
            fileDownloadName: file.Name);
    }
}