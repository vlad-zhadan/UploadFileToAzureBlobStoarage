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
        //private readonly IWebHostEnvironment _env;
        private readonly IFileService _fileService;
        private readonly ILogger<FileController> _logger;

        //IWebHostEnvironment env,
        public FileController(IFileService fileService, ILogger<FileController> logger)
        {
            //_env = env;
            _fileService = fileService;
            _logger = logger;
        }

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
            var result = await _fileService.UploadBlobAsync(fileInfo.File, fileInfo.Email);
            return Ok(result);
        }

        //[HttpPost]
        //public async Task<ActionResult<List<UploadResult>>> UploadFilesLocaly(List<IFormFile> files)
        //{
        //    List<UploadResult> uploadResults = new List<UploadResult>();

        //    foreach (var file in files)
        //    {
        //        var uploadResult = new UploadResult();
        //        string trustedFileNameForFileStorage;
        //        var untrustedFileName = file.FileName;
        //        uploadResult.FileName = untrustedFileName;
        //        //var trustedFileNameForDisplay = WebUtility.HtmlEncode(untrustedFileName);

        //        trustedFileNameForFileStorage = Path.GetRandomFileName();
        //        var path = Path.Combine(_env.ContentRootPath, "uploads", trustedFileNameForFileStorage);

        //        await using FileStream fs = new(path, FileMode.Create);
        //        await file.CopyToAsync(fs);

        //        uploadResult.StoredFileName = trustedFileNameForFileStorage;
        //        uploadResults.Add(uploadResult);
        //    }

        //    return Ok(uploadResults);
        //}



    }
}
