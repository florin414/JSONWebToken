namespace AuthentificationJWT.Api.Models.Response;

public class AuthenticationResponse
{
    public bool Success { get; set; } = default!;
    public string Token { get; set; } = default!;
    public IEnumerable<string>? Errors { get; set; }
    public string RefreshToken { get; set; }
}
