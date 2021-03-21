using System.Threading.Tasks;
using Visor.Abstractions.Entities.Results;

namespace Visor.Abstractions.User
{
    public interface ILoginManager
    {
        Task<LoginResult> Login(string username, string password, bool persist = false, bool lockoutEnabled = true);
        Task<LogoutResult> Logout(string logoutId);
        Task<LogoutResult> Logout(string logoutId = null, string returnUrl = null);
    }
}
