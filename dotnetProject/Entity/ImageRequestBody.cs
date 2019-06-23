using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace dotnetProject.RequestBody.Files
{
    public class ImageRequestBody
    {
        public IFormFile File { get; set; }
        public string Username { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        
    }
}
