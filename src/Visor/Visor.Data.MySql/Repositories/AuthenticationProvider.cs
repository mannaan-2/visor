using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visor.Abstractions.Entities.Results;
using Visor.Abstractions.Repositories;
using Visor.Data.MySql.Identity.Entities;

namespace Visor.Data.MySql.Repositories
{
    public class AuthenticationProvider : IAuthenticationProvider
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IIdentityServerInteractionService _interactionService;
        private readonly ILogger<AuthenticationProvider> _logger;

        public AuthenticationProvider(SignInManager<ApplicationUser> signInManager, 
            IIdentityServerInteractionService interactionService,
             ILogger<AuthenticationProvider> logger)
        {
            _signInManager = signInManager;
            _interactionService = interactionService;
            _logger = logger;
        }

       
        public async Task<LoginResult> VerifyLogin(string userName, string password, bool persist = false, bool lockOutEnabled = true)
        {
            var result = await _signInManager.PasswordSignInAsync(userName, password, persist, lockOutEnabled);
            return new LoginResult
            {
                Succeeded = result.Succeeded,
                IsLockedOut = result.IsLockedOut,
                IsNotAllowed = result.IsNotAllowed,
                RequiresTwoFactor = result.RequiresTwoFactor

            };
        }

        public async Task<LogoutResult> Logout(string logoutId)
        {
            var result = new LogoutResult { Errors = new List<Error>() };
            if (string.IsNullOrEmpty(logoutId))
            {
                result.Errors.Add(new Error { Code = "400", Message="requires logout Id" });
                return result;
            }
            var logoutCotext = await _interactionService.GetLogoutContextAsync(logoutId);
            if (logoutCotext == null || string.IsNullOrEmpty(logoutCotext.PostLogoutRedirectUri))
            {
                result.Errors.Add(new Error { Code = "400", Message = "Invalid logout id, context not found" });
                return result;
            }
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out. (client initiated)");
            result.Succeeded = true;
            result.ReturnUrl = logoutCotext.PostLogoutRedirectUri;
            return result;
        }

        public async Task<LogoutResult> Logout(string logoutId,string returnUrl = null)
        {
            var result = new LogoutResult { Errors = new List<Error>() };
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out. (user initiated)");
            result.Succeeded = true;
            result.ReturnUrl = returnUrl;
            return result;
        }
    }
}
