using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Threading.Tasks;
using Visor.Abstractions.Entities.Requests;
using Visor.Abstractions.Entities.Results;
using Visor.Abstractions.User;
using Visor.Tenancy.Abstractions;

namespace Visor.Api.Controllers
{
    [Route("[controller]")]
    [ApiVersion("1")]
    [ApiController]
    [Authorize("Bearer")]
    public class UserController : Controller
    {
        private readonly IRegistrationManager _registrationManager;
        private readonly ITenantContext _tenantContext;

        public UserController(IRegistrationManager registrationManager, ITenantContext tenantContext)
        {
            _registrationManager = registrationManager;
            _tenantContext = tenantContext;
        }
        [HttpGet("{username}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public string Get(string username)
        {
            return username;
        }
        /// <summary>
        /// Creates a user.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /user
        ///     {
        ///        "username": "abc",
        ///        "email": "abc@example.com",
        ///        "password": "superSecret1",
        ///        "emailConfirmed": false
        ///     }
        ///
        /// </remarks>
        /// <param name="user"></param>
        /// <returns>
        ///  /// Sample Response:
        ///
        ///     POST /user
        ///     {
        ///        "Succeeded": true,
        ///        "Errors": [{
        ///             "code": "",
        ///             "Message": ""
        ///        }]
        ///     }
        /// </returns>
        /// <response code="201">Returns the result object</response>
        /// <response code="400">If the data is invalid</response>   
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult>> Post([FromBody] UserCreationRequest user)
        {
            var result = await _registrationManager.Register(user.Username, user.Email, user.Password, !user.EmailConfirmed);
            if (!result.Succeeded)
                return BadRequest(result);
            return CreatedAtAction(nameof(Get), new { username = user.Username }, result);
        }
    }
}
