using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Visor.Api.Controllers.Api
{
    [Route("[controller]")]
    [ApiVersion("1")]
    [ApiVersion("2")]
    [ApiController]
    public class VersionExampleController : ControllerBase
    {
        [HttpGet]
       // [HttpGet("version"), MapToApiVersion("1")] //Maps by default
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [HttpGet]
        [HttpGet("version"), MapToApiVersion("2")]
        public IEnumerable<string> Get2()
        {
            return new string[] { "Version2", "value2" };
        }
        // GET api/<RegisterController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<RegisterController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<RegisterController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<RegisterController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
