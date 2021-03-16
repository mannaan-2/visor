

namespace Visor.Abstractions.Entities.Config.Identity
{
    public class ApplicationIdentitySettings
    {
        public UserSettings User { get; set; }
        public PasswordSettings Password { get; set; }
        public LockoutSettings Lockout { get; set; }
        public SignInSettings SignIn { get; set; }
    }
}
