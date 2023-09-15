﻿using Azure.Storage.Blobs;
using BlazorApp1.Server.Controllers;
using BlazorApp1.Server.Services;
using BlazorApp1.Shared;
using FakeItEasy;
using FluentAssertions;
using FunctionApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UploadFile.Test.Controllers
{
    public class FileControllerTest
    {
        private readonly IFileService _fileService;
        private readonly ILogger<FileController> _logger;
        private readonly IValidationEmailService _validationEmailService;
        public FileControllerTest()
        {
            _fileService = A.Fake< IFileService>();
            _logger = A.Fake<ILogger<FileController>>();
            _validationEmailService = A.Fake<IValidationEmailService>();
        }

        [Fact]
        public async Task UploadFileToAzureBlob_SendingValidInput_ReturnsOkResult()
        {
            // Arrange
            var fileInfo = new UploadResult
            {
                File = new FormFile(Stream.Null, 0, 0, "testFile", "testFile.docx"),
                Email = "test@gmail.com"
            };
            var blobResponse = A.Fake<BlobResponseDto>();

            A.CallTo(() => _validationEmailService.IsValidateEmail(fileInfo.Email)).Returns(true);

            var controller = new FileController(_fileService, _logger, _validationEmailService);

            // Act
            var result = await controller.UploadFileToAzureBlob(fileInfo) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeEquivalentTo(blobResponse);
        }
        [Fact]
        public async Task UploadFileToAzureBlob_SendingEmptyFileName_ReturnsBadRequest()
        {
            // Arrange
            var fileInfo = new UploadResult
            {
                File = new FormFile(Stream.Null, 0, 0, null, null),
                Email = "test@gmail.com"
            };

            var controller = new FileController(_fileService, _logger, _validationEmailService);

            // Act
            var result = await controller.UploadFileToAzureBlob(fileInfo) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(400); 
            result.Value.Should().Be("Invalid file name");
        }

        [Fact]
        public async Task UploadFileToAzureBlob_SendingEmptyEmail_ReturnsBadRequest()
        {
            // Arrange
            var fileInfo = new UploadResult
            {
                File = new FormFile(Stream.Null, 0, 0, "testFile", "testFile.docx"),
                Email = ""
            };

            var controller = new FileController(_fileService, _logger, _validationEmailService);

            // Act
            var result = await controller.UploadFileToAzureBlob(fileInfo) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(400);
            result.Value.Should().Be("Email cannot be empty");
        }
    }
    
}
