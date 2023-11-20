namespace AuthentificationJWT.Api.Middlewares;

public class JwtMiddleware
{
    private readonly RequestDelegate next;
    private JwtOptions JwtOptions { get; init; }

    public JwtMiddleware(RequestDelegate next, IOptions<JwtOptions> options)
    {
        this.next = next;
        JwtOptions = options.Value;
    }

    public async Task Invoke(HttpContext context, IRepository<User, string> userRepository)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token != null)
            await AttachUserToContext(context, token, userRepository);

        await next(context);
    }

    private async Task AttachUserToContext(
        HttpContext context,
        string token,
        IRepository<User, string> userRepository
    )
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            tokenHandler.ValidateToken(
                token,
                new TokenValidationParameters
                {
                    ValidIssuer = this.JwtOptions.Issuer,
                    ValidAudience = this.JwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(this.JwtOptions.Key!)
                    ),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                },
                out SecurityToken validatedToken
            );

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = jwtToken.Claims.First(c => c.Type == "sub").Value;

            context.Items["User"] = await userRepository.GetById(userId);
        }
        catch { }
    }
}
