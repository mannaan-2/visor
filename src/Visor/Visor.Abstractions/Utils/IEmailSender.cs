using System.Threading.Tasks;

namespace Visor.Abstractions.Utils
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
        Task SendEmailAsync(string email, string subject, string message, string from, string fromName);
    }
}
