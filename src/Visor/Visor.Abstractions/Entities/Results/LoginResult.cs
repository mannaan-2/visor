namespace Visor.Abstractions.Entities.Results
{
    public class LoginResult :BaseResult
    {
        //
        // Summary:
        //     Returns a flag indication whether the user attempting to sign-in is locked out.
        //
        // Value:
        //     True if the user attempting to sign-in is locked out, otherwise false.
        public bool IsLockedOut { get;  set; }
        //
        // Summary:
        //     Returns a flag indication whether the user attempting to sign-in is not allowed
        //     to sign-in. e.g email not confirmed
        //
        // Value:
        //     True if the user attempting to sign-in is not allowed to sign-in, otherwise false.
        public bool IsNotAllowed { get;  set; }
        //
        // Summary:
        //     Returns a flag indication whether the user attempting to sign-in requires two
        //     factor authentication.
        //
        // Value:
        //     True if the user attempting to sign-in requires two factor authentication, otherwise
        //     false.
        public bool RequiresTwoFactor { get;  set; }
    }
}
