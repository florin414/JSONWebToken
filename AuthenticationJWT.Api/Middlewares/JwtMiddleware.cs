namespace AuthentificationJWT.Api.Middlewares;

public class JwtMiddleware
{
    private readonly RequestDelegate next;
    private JwtOptions JwtOptions { get; init; }

    public JwtMiddleware(RequestDelegate next, IOptions<JwtOptions> options)
    {
        this.next = next;
        this.JwtOptions = options.Value;
    }

    public async Task Invoke(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token != null)
            AttachUserToContext(context, token);

        await next(context);
    }

    private void AttachUserToContext(HttpContext context, string token)
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

            context.Items["User"] = new User
            {
                UserName = "Aamir Khan",
                Email = "AamirKhan@Example.com",
                Password = "Allah",
                Role = "Admin"
            };
        }
        catch { }
    }
}
