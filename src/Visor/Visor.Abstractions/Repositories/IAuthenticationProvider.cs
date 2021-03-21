using System.Threading.Tasks;
using Visor.Abstractions.Entities.Results;

namespace Visor.Abstractions.Repositories
{
    public interface IAuthenticationProvider
    {
        Task<LoginResult> VerifyLogin(string userName, string password, bool persist = false, bool lockOutEnabled = true);
        Task<LogoutResult> Logout(string logoutId);
        Task<LogoutResult> Logout(string logoutId=null,string returnUrl = null);
    }
}
