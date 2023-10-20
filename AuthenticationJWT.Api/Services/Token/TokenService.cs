namespace AuthentificationJWT.Api.Services.Token;

public class TokenService : ITokenService
{
    private JwtOptions JwtOptions { get; init; }

    public TokenService(IOptions<JwtOptions> options)
    {
        this.JwtOptions = Guard.ArgumentNotNull(options.Value, nameof(options));
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
        var key = Encoding.UTF8.GetBytes(this.JwtOptions.Key!);
        var secret = new SymmetricSecurityKey(key);
        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    private static Claim[] GetClaims(CreateUserAuthToken createUserAuthToken)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, createUserAuthToken.Id),
            new Claim(JwtRegisteredClaimNames.Email, createUserAuthToken.Email ?? ""),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, createUserAuthToken.Role ?? ""),
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
            issuer: this.JwtOptions.Issuer,
            audience: this.JwtOptions.Audience,
            claims,
            expires: DateTime.Now.AddSeconds(Convert.ToDouble(this.JwtOptions.TokenLifetime)),
            signingCredentials: signingCredentials
        );

        return tokenOptions;
    }
}
