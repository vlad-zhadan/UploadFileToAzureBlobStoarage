using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp1.Shared
{
    public class UploadResult
    {
        //public string? FileName { get; set; }
        //public string? StoredFileName { get; set; }

        public IFormFile? File { get; set; }
        public string? Email { get; set; }
        
        
    }
}
    