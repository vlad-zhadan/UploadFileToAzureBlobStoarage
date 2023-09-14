using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BlazorApp1.Shared;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using Microsoft.AspNetCore.Http;

namespace BlazorApp1.Server.Services
{
    public class FileService : IFileService
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

        public async Task<BlobResponseDto> UploadBlobAsync(IFormFile blob, string email)
        {
            BlobResponseDto response = new BlobResponseDto();
            BlobClient client = _filesConteiner.GetBlobClient(blob.FileName);

            //add metadata
            //client.SetMetadata(new Dictionary<string, string>
            //{
            //    { "Email", email },
            //    { "Name",  blob.FileName} 
            //});

            //BlobUploadOptions options = new BlobUploadOptions
            //{
            //    Metadata = new Dictionary<string, string>
            //    {
            //        { "Email", email },
            //        { "Name",  blob.FileName}
            //    }
            //};

            await using (Stream? data = blob.OpenReadStream())
            {
                //, options
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
