using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Threading.Tasks;
using Visor.Abstractions.Entities.Requests;
using Visor.Abstractions.Entities.Results;
using Visor.Abstractions.User;

namespace Visor.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize("Bearer")]
    public class LogoutController : Controller
    {
        private readonly ILoginManager _loginManager;

        public LogoutController(ILoginManager loginManager)
        {
            _loginManager = loginManager;
        }
        /// <summary>
        /// Logout.
        /// </summary>
        /// <param name="logoutId">logout id</param>
        /// <returns>
        ///
        /// </returns>
        /// <response code="200">Logged out</response>
        /// <response code="400">Invalid action</response>   
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LogoutResult>> Get( string logoutId)
        {
            var result = await _loginManager.Logout(logoutId); ;
            if (!result.Succeeded)
                return BadRequest(result);
            return Ok(result);
        }
        /// <summary>
        /// Logout.
        /// </summary>
        /// <param name="logoutId">logout id (optional)</param>
        /// <param name="returnUrl">Return Url (optional)</param>
        /// <returns>
        ///
        /// </returns>
        /// <response code="200">Logged out</response>
        /// <response code="400">Invalid action</response>   
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LogoutResult>> Post(string returnUrl = null, string logoutId = null)
        {
             var result = await _loginManager.Logout(logoutId, returnUrl); ;
            if (!result.Succeeded)
                return BadRequest(result);
            return Ok(result);
        }

    }
}
