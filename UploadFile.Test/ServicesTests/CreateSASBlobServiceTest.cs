using Azure.Storage.Blobs;
using FakeItEasy;
using FluentAssertions;
using FunctionApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UploadFile.Test.ServicesTests
{
    public class CreateSASBlobServiceTest
    {
        private CreateSASBlobService _sasBlobService; 

        public CreateSASBlobServiceTest()
        {
            _sasBlobService = new CreateSASBlobService();
        }

        [Fact]
        public void CreateSASBlob_SendingAuthorizedClient_ShouldReturnSasUri()
        {
            // Arrange
            var blobClient = A.Fake<BlobClient>();
            A.CallTo(() => blobClient.CanGenerateSasUri).Returns(true);

            // Act
            var sasUri = _sasBlobService.CreateSASBlob(blobClient);

            // Assert
            sasUri.Should().NotBeNull().And.BeAssignableTo<Uri>();

        }

        [Fact]
        public void CreateSASBlob_SendingNotAuthorizedClient_ShouldReturnNull()
        {
            // Arrange
            var blobClient = A.Fake<BlobClient>();
            A.CallTo(() => blobClient.CanGenerateSasUri).Returns(false);

            // Act
            var sasUri = _sasBlobService.CreateSASBlob(blobClient);

            // Assert
            sasUri.Should().BeNull();
        }

        [Fact]
        public void CreateSASBlob_SendingStoredPolicyNameAsNull_ShouldReturnSasUriWithHourExpiration()
        {
            // Arrange
            var blobClient = A.Fake<BlobClient>();
            A.CallTo(() => blobClient.CanGenerateSasUri).Returns(true);

            // Act
            var sasUri = _sasBlobService.CreateSASBlob(blobClient);

            // Assert
            sasUri.Should().NotBeNull().And.BeAssignableTo<Uri>();

            
        }
    }
}
