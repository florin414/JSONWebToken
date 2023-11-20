namespace AuthenticationJWT.Api.BusinessLogic.Services.Token;

public class TokenService : ITokenService
{
    private JwtOptions JwtOptions { get; init; }

    public TokenService(IOptions<JwtOptions> options)
    {
        JwtOptions = Guard.ArgumentNotNull(options.Value, nameof(options));
    }

    public (string signature, string id) CreateToken(CreateUserAuthToken createUserAuthToken)
    {
        var signingCredentials = GetSigningCredentials();
        var claims = GetClaims(createUserAuthToken);
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
        return (new JwtSecurityTokenHandler().WriteToken(tokenOptions), tokenOptions.Id);
    }

    private SigningCredentials GetSigningCredentials()
    {
        var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions.Key));
        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    private static Claim[] GetClaims(CreateUserAuthToken createUserAuthToken)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, createUserAuthToken.Id),
            new Claim(JwtRegisteredClaimNames.Email, createUserAuthToken.Email ?? ""),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("id", createUserAuthToken.Id),
        };

        return claims;
    }

    private JwtSecurityToken GenerateTokenOptions(
        SigningCredentials signingCredentials,
        Claim[] claims
    )
    {
        var tokenOptions = new JwtSecurityToken(
            issuer: JwtOptions.Issuer,
            audience: JwtOptions.Audience,
            claims,
            expires: DateTime.UtcNow.AddHours(Convert.ToDouble(JwtOptions.TokenLifetime)),
            signingCredentials: signingCredentials
        );

        return tokenOptions;
    }
}
