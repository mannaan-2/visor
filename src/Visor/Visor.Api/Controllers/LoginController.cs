using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Visor.Abstractions.Entities.Requests;
using Visor.Abstractions.Entities.Results;
using Visor.Abstractions.User;
using Visor.Data.MySql.Identity.Entities;
using Visor.Tenancy.Abstractions;

namespace Visor.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize("Bearer")]
    public class LoginController : Controller
    {
        private readonly ILoginManager _loginManager;

        public LoginController(ILoginManager loginManager)
        {
            _loginManager = loginManager;
        }
        /// <summary>
        /// Login.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /login
        ///     {
        ///        "username": "abc",
        ///        "password": "superSecret1"
        ///     }
        ///
        /// </remarks>
        /// <param name="login"></param>
        /// <returns>
        ///
        /// </returns>
        /// <response code="200">Login successful</response>
        /// <response code="404">requested credentials could not be validated</response>   
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LoginResult>> Post([FromBody] LoginRequest login)
        {
            var result = await _loginManager.Login(login.Username, login.Password, login.Persist ?? true); ;
            if (!result.Succeeded)
                return NotFound(result);
            return Ok(result);
        }

    }
}
