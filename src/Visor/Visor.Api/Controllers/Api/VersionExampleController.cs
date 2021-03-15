using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mime;
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public string Get(int id)
        {
            return "value";
        }

        /// <summary>
        /// Creates a TodoItem.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Todo
        ///     {
        ///        "id": 1,
        ///        "name": "Item1",
        ///        "isComplete": true
        ///     }
        ///
        /// </remarks>
        /// <param name="item"></param>
        /// <returns>A newly created TodoItem</returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>   
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<TodoItem> Post([FromBody] TodoItem item)
        {
            if (item.Id == -1)
                return BadRequest();
            return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
        }

        
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Put(int id, [FromBody] string value)
        {

            return BadRequest();
            //return Ok();
        }

        /// <summary>
        /// Deletes a specific TodoItem.
        /// </summary>
        /// <param name="id"></param>     
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        /// <summary>
        /// gets a second Item
        /// </summary>
        /// <param name="id"></param>     
        [HttpGet("user/{id}")]
        [Authorize]
        public void UserInfo(string id)
        {
        }
    }

    public class TodoItem
    {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [DefaultValue(false)]
        public bool IsComplete { get; set; }
    }
}
