namespace AuthenticationJWT.Api.Domain.Models.Auth;

public class CreateUserAuthToken
{
    public string Id { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string? Role { get; set; }
}
