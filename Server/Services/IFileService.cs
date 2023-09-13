using BlazorApp1.Shared;

namespace BlazorApp1.Server.Services
{
    public interface IFileService
    {
        Task<List<BlobDto>> ListBlobsAsync();
        Task<BlobResponseDto> UploadBlobAsync(IFormFile blob);
    }
}