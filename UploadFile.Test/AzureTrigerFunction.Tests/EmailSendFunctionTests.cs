using Castle.Core.Logging;
using FakeItEasy;
using FluentAssertions;
using FunctionApp;
using FunctionApp.Services;
using Microsoft.Extensions.Logging;
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
        public void Run_WhenMetadataContainsEmail_ShouldNotCallSendEmail()
        {
            // Arrange
            var email = "test@gmail.com";
            var uri = "http://example.com";
            var metadata = new Dictionary<string, string>
            {
                { "Email", email },
                { "Uri", uri }
            };

            // Create a fake ISendEmaisService that does nothing
            var sendEmailService = A.Fake<ISendEmaisService>();
            A.CallTo(() => sendEmailService.SendEmail(A<string>._, A<string>._)).Returns(new ResponseEmailDto { IsSent = true });

            var logger = A.Fake<ILogger<EmailSendFunction>>();

            var function = new EmailSendFunction(sendEmailService);

            // Act
            function.Run(new MemoryStream(), "testblob", metadata, logger);

            // Assert
             
           
        }
        
    }
}
