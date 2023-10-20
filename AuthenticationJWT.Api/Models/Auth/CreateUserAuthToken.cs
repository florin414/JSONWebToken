namespace AuthentificationJWT.Api.Models.Auth;

public class CreateUserAuthToken
{
    public string Id { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string? Email { get; set; }
    public string? Role { get; set; }
}
