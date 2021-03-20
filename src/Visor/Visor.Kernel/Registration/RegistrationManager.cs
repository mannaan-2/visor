using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Visor.Abstractions.Entities.Results;
using Visor.Abstractions.Repositories;
using Visor.Abstractions.User;
using Visor.Abstractions.Utils;

namespace Visor.Kernel.Registration
{
    public class RegistrationManager : IRegistrationManager
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<RegistrationManager> _logger;

        public RegistrationManager(IUserRepository userRepository,IEmailSender emailSender, ILogger<RegistrationManager> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
            _emailSender = emailSender;
        }
        public async Task<BaseResult> Register(string email, string password, bool requiresConfirmation = true)
        {
            return await Register(string.Empty, email, password, requiresConfirmation);
        }
        public async Task<BaseResult> Register(string userName, string email, string password, bool requiresConfirmation = true)
        {
            var result = await _userRepository.CreateLogin(
                string.IsNullOrEmpty(userName) ? email:userName, 
                email,
                password,
                !requiresConfirmation) ;
           
            return result;
        }
        public async Task<BaseResult> SendEmailConfirmationLink(string userName)
        {
            var result = new BaseResult() { Errors = new List<Error>()};
            try{
                var code = await _userRepository.GetEmailConfirmationCode(userName);
                var user = await _userRepository.Find(userName);
                var callbackUrl = $"https://local:111?user={user}&code={code}";
                await _emailSender.SendEmailAsync(user, "Confirm your email",
                     $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
            }
            catch(InvalidOperationException ex)
            {
                result.Errors.Add(new Error { Code="404", Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                result.Errors.Add(new Error { Code = "500",Message ="Could not send verification email" });
            }
            result.Succeeded = true;
            return result;
        }

    }
}
