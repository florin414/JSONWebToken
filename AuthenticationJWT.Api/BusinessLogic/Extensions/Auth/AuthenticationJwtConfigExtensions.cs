namespace AuthenticationJWT.Api.BusinessLogic.Extensions.Auth;

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
            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey = securityKey,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero,
        };

        services.AddSingleton(tokenValidationParameters);

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.TokenValidationParameters = tokenValidationParameters;
                x.SaveToken = true;
                x.RequireHttpsMetadata = false;
                
            });

        return services;
    }
}
