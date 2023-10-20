namespace AuthentificationJWT.Api.Models.UserModels;

public class User : IdentityUser<string>
{
    public string Password { get; set; }
    public string? Role { get; set; }
}
