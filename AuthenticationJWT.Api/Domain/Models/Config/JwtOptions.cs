namespace AuthenticationJWT.Api.Domain.Models.Config;

public class JwtOptions
{
    public const string JwtSettings = "JwtSettings";
    public string Issuer { get; set; } = default!;
    public string Audience { get; set; } = default!;
    public string Key { get; set; } = default!;
    public string TokenLifetime { get; set; } = default!;
}
