namespace AuthentificationJWT.Api.Models.Request;

public class UserAuthenticateRequest
{
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
