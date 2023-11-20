namespace AuthenticationJWT.Api.Domain.Models.UserModels;

public class User : IdentityUser
{
    public string? Password { get; set; }
    public string? Role { get; set; }
}
