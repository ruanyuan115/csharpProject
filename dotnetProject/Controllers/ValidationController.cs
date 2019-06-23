using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using dotnetProject.Infrastructure.Services;
using dotnetProject.RequestBody.Validation;
using dotnetProject.RespondInfo.Validation;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace dotnetProject.Controllers
{
    [Route("api/[controller]")]
    public class ValidationController : Controller
    {
        private readonly IValidationService _validationService;

        public ValidationController(IValidationService validationService )
        {
            _validationService = validationService;
        }
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            String str = "test";
            return new string[] { "value1", "value2",str };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [Route("login")]
        [HttpPost]
        public async Task<ActionResult<Boolean>> Post([FromBody]ValidationBody value)
        {
            Boolean flag = await _validationService.ValidationAsync(value.Username, value.Password);
            return Ok(flag);
        }

        [Route("registe")]
        [HttpPost]
        public async Task<ActionResult<RegisteRespond>> Post([FromBody] RegisteBody value)
        {
            RegisteRespond registeRespond= await _validationService.RegisteAsync(value.Username, value.Password, value.Mail, value.Phone);
            return Ok(registeRespond);
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
