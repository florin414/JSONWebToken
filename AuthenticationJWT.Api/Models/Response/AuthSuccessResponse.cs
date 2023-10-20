namespace AuthentificationJWT.Api.Models.Response;

public class AuthSuccessResponse
{
    public string Token { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
}
