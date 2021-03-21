using System;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Visor.Abstractions.Entities.Results;
using Visor.Abstractions.Repositories;
using Visor.Abstractions.User;
using Visor.Abstractions.Utils;

namespace Visor.Kernel.Registration
{
    public class LoginManager : ILoginManager
    {
        private readonly IAuthenticationProvider _authProvider;
        private readonly ILogger<RegistrationManager> _logger;

        public LoginManager(IAuthenticationProvider authProvider, ILogger<RegistrationManager> logger)
        {
            _authProvider = authProvider;
            _logger = logger;
        }


        public async Task<LoginResult> Login(string username, string password, bool persist = false, bool lockoutEnabled = true)
        {
          return await  _authProvider.VerifyLogin(username, password, persist, lockoutEnabled);
        }

        public async Task<LogoutResult> Logout(string logoutId)
        {
            return await _authProvider.Logout(logoutId);
        }

        public async Task<LogoutResult> Logout(string logoutId = null, string returnUrl = null)
        {
            return await _authProvider.Logout(logoutId, returnUrl);
        }
    }
}
