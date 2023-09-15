using Azure.Storage.Blobs;
using BlazorApp1.Server.Services;
using FakeItEasy;
using FluentAssertions;
using FunctionApp.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UploadFile.Test.ServicesTests
{
    public class FileServiceTest
    {
        [Fact]
        public async Task UploadBlobAsync_ValidFile_UploadsFileAndReturnsResponse()
        {
            // Arrange
            var email = "test@example.com";
            var formFile = A.Fake<IFormFile>();
            A.CallTo(() => formFile.FileName).Returns("test.docx");
            var blobStream = new MemoryStream(new byte[] { 1, 2, 3 });

            var createSASBlobService = A.Fake<ICreateSASBlobService>();
            //A.CallTo(() => createSASBlobService.CreateSASBlob(A<BlobClient>._)).Returns(new System.Uri("https://example.com/sas"));

            var fileService = new FileService(createSASBlobService);

            // Act
            var response = await fileService.UploadBlobAsync(formFile, email);

            // Assert
            response.Should().NotBeNull();
            response.Status.Should().Be("File test.docx uploaded to Azure");
            response.Error.Should().BeFalse();
            response.Blob.Should().NotBeNull();
            response.Blob.Uri.Should().Be("https://example.com/sas");
            response.Blob.Name.Should().Be("test.docx");
        }
    }
}
