namespace Visor.Abstractions.Entities.Requests
{
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool? Persist { get; set; }
    }
}
