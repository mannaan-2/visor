using System;
using System.Threading.Tasks;
using Visor.Abstractions.Entities.Results;

namespace Visor.Abstractions.Repositories
{
    public interface IUserRepository
    {
        Task<BaseResult> CreateLogin(string userName, string email, string password);
        Task<BaseResult> CreateLogin(string userName, string email, string password, bool emailPreApproved);
        Task<string> GetEmailConfirmationCode(string userName);
        Task<string> Find(string userName);
    }
}
