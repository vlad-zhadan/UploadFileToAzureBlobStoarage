using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BlazorApp1.Shared;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using Microsoft.AspNetCore.Http;
using Azure;
using System.Net;
using System.Text;
using FunctionApp.Services;

namespace BlazorApp1.Server.Services
{
    public class FileService : IFileService
    {
        private readonly string _stoarageAccount = "uploadfilefortestproject";
        private readonly string _key = "o502kMzHchFXQAPn6Dmk6FTOG2jITh84gMtBjjiKEXFJIdKy9gdIrlQJ2WXJCvF40wpZEit4ToHX+AStw6mH6g==";
        private readonly BlobContainerClient _filesConteiner;
        private readonly ICreateSASBlobService _createSASBlob;

        public FileService(ICreateSASBlobService createSASBlob)
        {
            var credential = new Azure.Storage.StorageSharedKeyCredential(_stoarageAccount, _key);
            var blobUri = $"https://{_stoarageAccount}.blob.core.windows.net";
            var blobServiceClient = new BlobServiceClient(new Uri(blobUri), credential);
            _filesConteiner = blobServiceClient.GetBlobContainerClient("files");
            _createSASBlob = createSASBlob;
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

            var trustedFileNameForFileStorage = Path.GetRandomFileName();
            BlobClient client = _filesConteiner.GetBlobClient(trustedFileNameForFileStorage);
          
            await using (Stream? data = blob.OpenReadStream())
            {
                await client.UploadAsync(data);
            }


            Uri uri = _createSASBlob.CreateSASBlob(client);
             

            IDictionary<string, string> metadata = new Dictionary<string, string>
            {
                { "Email", email },
                { "Name", Encoding.ASCII.GetString(Encoding.UTF8.GetBytes(blob.FileName)) },
                { "Uri", uri.ToString() }
            };


            await client.SetMetadataAsync(metadata);

            response.Status = $"File {blob.FileName} uploaded !";
            response.Error = false;
            response.Blob.Uri = client.Uri.AbsoluteUri;
            response.Blob.Name = client.Name;

            return response;
        }
    }

}
