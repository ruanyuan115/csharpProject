using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Runtime.InteropServices;
using dotnetProject.Infrastructure.Services;

using dotnetProject.RequestBody.Files;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace dotnetProject.Controllers
{
    [Route("api/[controller]")]
    public class FileController : Controller
    {
        private readonly IFileService _fileService;
        private readonly IInteropService _interopService;

        public FileController(IFileService fileService,IInteropService interopService)
        {
            _fileService = fileService;
            _interopService = interopService;
        }
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {

            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return _interopService.Message();
        }

        // POST api/<controller>
        [HttpPost]
        [Route("uploadimage")]

        public async Task<ActionResult<string>> Post([FromForm]ImageRequestBody value)
        {
            var result = await _fileService.UploadImage(value.File);
            return Ok(result);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
