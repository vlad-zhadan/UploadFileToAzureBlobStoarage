using Castle.Core.Logging;
using FakeItEasy;
using FunctionApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrigerFunction;

namespace UploadFile.Test.AzureTrigerFunction.Tests
{
    public class EmailSendFunctionTests
    {
        [Fact]
        public void Run_WithValidMetadata_ShouldSendEmail()
        {
            // Arrange
            var mockSendEmailService = A.Fake<ISendEmaisService>();
            var logger = A.Fake<ILogger>();
            var emailSendFunction = new EmailSendFunction(mockSendEmailService);

            var metadata = new Dictionary<string, string>
            {
                { "Email", "test@example.com" },
                { "Name", "Test File" },
                { "Uri", "https://example.com/file" }
            };

            var blobStream = new MemoryStream();
            var blobName = "testfile.txt";

            // Act
            emailSendFunction.Run(blobStream, blobName, metadata, (Microsoft.Extensions.Logging.ILogger)logger);

            // Assert
            A.CallTo(() => mockSendEmailService.SendEmail("test@example.com", "https://example.com/file"))
                .MustHaveHappenedOnceExactly();


        }

        [Fact]
        public void Run_WithMissingMetadata_ShouldNotSendEmail()
        {
            // Arrange
            var mockSendEmailService = A.Fake<ISendEmaisService>();
            var logger = A.Fake<ILogger>();
            var emailSendFunction = new EmailSendFunction(mockSendEmailService);

            var metadata = new Dictionary<string, string>
            {
                // Missing Email, Name, and Uri
            };

            var blobStream = new MemoryStream();
            var blobName = "testfile.txt";

            // Act
            emailSendFunction.Run(blobStream, blobName, metadata, (Microsoft.Extensions.Logging.ILogger)logger);

            // Assert
            A.CallTo(() => mockSendEmailService.SendEmail(A<string>._, A<string>._)).MustNotHaveHappened();
        }
    }
}
