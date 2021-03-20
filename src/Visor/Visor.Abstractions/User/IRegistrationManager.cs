using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visor.Abstractions.Entities.Results;

namespace Visor.Abstractions.User
{
    public interface IRegistrationManager
    {
        Task<BaseResult> Register(string email, string password, bool requiresConfirmation = true);
        Task<BaseResult> Register(string userName, string email, string password, bool requiresConfirmation = true);
        Task<BaseResult> SendEmailConfirmationLink(string userName);
    }
}
