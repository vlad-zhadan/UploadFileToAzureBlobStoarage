using Azure.Storage.Blobs;
using BlazorApp1.Shared;

namespace BlazorApp1.Server.Services
{
    public class FileService
    {
        private readonly string _stoarageAccount = "uploadfilefortestproject";
        private readonly string _key = "o502kMzHchFXQAPn6Dmk6FTOG2jITh84gMtBjjiKEXFJIdKy9gdIrlQJ2WXJCvF40wpZEit4ToHX+AStw6mH6g==";
        private readonly BlobContainerClient _filesConteiner;

        public FileService()
        {
            var credential = new Azure.Storage.StorageSharedKeyCredential(_stoarageAccount, _key);
            var blobUri = $"https://{_stoarageAccount}.blob.core.windows.net";
            var blobServiceClient = new BlobServiceClient(new Uri(blobUri), credential);
            _filesConteiner = blobServiceClient.GetBlobContainerClient("files");
        }

        public async Task<List<BlobDto>> ListBlobsAsync()
        {
            List<BlobDto> files = new List<BlobDto>();

            string uri = _filesConteiner.Uri.ToString();
            await foreach (var file in _filesConteiner.GetBlobsAsync())
            {
                var name = file.Name;
                var fullUri = $"{uri}/{name}";

                files.Add(new BlobDto
                {
                    Name = name,
                    Uri = fullUri,
                    ContentType = file.Properties.ContentType
                });
            }
            return files;
        }

        public async Task<BlobResponseDto> UploadBlobAsync(IFormFile blob)
        {
            BlobResponseDto response = new BlobResponseDto();
            BlobClient client = _filesConteiner.GetBlobClient(blob.FileName);

            await using (Stream? data = blob.OpenReadStream())
            {
                await client.UploadAsync(data);
            }

            response.Status = $"File {blob.FileName} uploaded !";
            response.Error = false;
            response.Blob.Uri = client.Uri.AbsoluteUri;
            response.Blob.Name = client.Name;

            return response;
        }
    }

}
