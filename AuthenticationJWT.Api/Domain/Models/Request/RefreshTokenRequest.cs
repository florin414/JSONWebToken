namespace AuthenticationJWT.Api.Domain.Models.Request;

public class RefreshTokenRequest
{
    public required string Token { get; set; } = string.Empty;
    public required string RefreshToken { get; set; } = string.Empty;
}
