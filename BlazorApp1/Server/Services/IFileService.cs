using BlazorApp1.Shared;

namespace BlazorApp1.Server.Services
{
    public interface IFileService
    {
        Task<BlobResponseDto> UploadBlobAsync(IFormFile blob, string email);
    }
}