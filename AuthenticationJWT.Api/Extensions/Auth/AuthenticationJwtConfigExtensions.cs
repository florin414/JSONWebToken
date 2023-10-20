namespace AuthentificationJWT.Api.Extensions.Auth;

public static class AuthenticationJwtConfigExtensions
{
    public static IServiceCollection AddAuthenticationJwt(
        this IServiceCollection services,
        IConfiguration config
    )
    {
        var jwtOptions = config.GetSection(JwtOptions.JwtSettings).Get<JwtOptions>();

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions!.Key));

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = jwtOptions!.Issuer,
            ValidAudience = jwtOptions!.Audience,
            IssuerSigningKey = securityKey,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
        };

        services.AddSingleton(tokenValidationParameters);

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(x => x.TokenValidationParameters = tokenValidationParameters);

        return services;
    }
}
