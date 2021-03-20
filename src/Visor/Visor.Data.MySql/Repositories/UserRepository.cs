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
    public class UserRepository: IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        //private readonly ILogger<UserRepository> _logger;

        public UserRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<BaseResult> CreateLogin(string userName, string email, string password)
        {
            return await CreateLogin(userName, email, password, false);
        }
        public async Task<BaseResult> CreateLogin(string userName, string email, string password, bool emailPreApproved)
        {
            var user = new ApplicationUser { UserName = userName, Email = email, EmailConfirmed = emailPreApproved};
           
            var result = await _userManager.CreateAsync(user, password);
            return new BaseResult { 
                Succeeded = result.Succeeded, 
                Errors = result.Errors.Select(e => new Error { Code = e.Code, Message = e.Description }).ToList() 
            };
        }
        public async Task<string> GetEmailConfirmationCode(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                throw new InvalidOperationException("Code not generated. User could not be found");
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            return code;
        }
        public async Task<string> Find(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                throw new InvalidOperationException("User could not be found");
            
            return user.Email;
        }
    }
}
