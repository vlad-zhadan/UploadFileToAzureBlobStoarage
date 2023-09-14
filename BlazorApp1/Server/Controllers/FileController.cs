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

        
        public FileController(IFileService fileService, ILogger<FileController> logger)
        {
         
            _fileService = fileService;
            _logger = logger;
        }

        // delete this get !!!!
        [HttpGet]
        public async Task<IActionResult> ListAllBlobsAsync()
        {
            var result = await _fileService.ListBlobsAsync();
            return Ok(result);
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

            if (fileInfo.Email == "")
            {
                return BadRequest("Email cannot be empty");
            }

            var result = await _fileService.UploadBlobAsync(fileInfo.File, fileInfo.Email);
            return Ok(result);
        }
    }
}
