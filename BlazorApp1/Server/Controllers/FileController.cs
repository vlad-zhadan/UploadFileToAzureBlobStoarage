using Azure.Core.GeoJson;
using BlazorApp1.Server.Services;
using BlazorApp1.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;

namespace BlazorApp1.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly ILogger<FileController> _logger;
        private readonly IValidationEmailService _validationEmailService;

        public FileController(IFileService fileService, ILogger<FileController> logger, IValidationEmailService validationEmailService)
        {
         
            _fileService = fileService;
            _logger = logger;
            _validationEmailService = validationEmailService;
        }

        [HttpPost]
        public async Task<IActionResult> UploadFileToAzureBlob([FromForm] UploadResult fileInfo)
        {
            _logger.LogInformation(fileInfo.File.FileName);
            _logger.LogInformation(fileInfo.Email);

            if(fileInfo.File.FileName is null)
            {
                return BadRequest("Invalid file name");
            }

            var allowedExtension = ".docx";
            var fileExtension = Path.GetExtension(fileInfo.File.FileName).ToLowerInvariant();

            if (fileExtension != allowedExtension)
            {
                return BadRequest("Invalid file extension. Please upload a .docx file.");
            }

            if (fileInfo.Email == "")
            {
                return BadRequest("Email cannot be empty");
            }

            if (!_validationEmailService.IsValidateEmail(fileInfo.Email))
            {
                return BadRequest("Invalid email");
            }

            var result = await _fileService.UploadBlobAsync(fileInfo.File, fileInfo.Email);
            return Ok(result);
        }
    }
}
