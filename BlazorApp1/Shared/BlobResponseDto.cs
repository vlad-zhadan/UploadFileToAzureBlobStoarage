﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp1.Shared
{
    
    public class BlobResponseDto
    {
        public BlobResponseDto()
        {
            Blob = new BlobDto();
        }
        public string? Status { get; set; }
        public bool Error { get; set; }
        public BlobDto Blob { get; set; }
        public string? EmailTo { get; set; }
    }
}
