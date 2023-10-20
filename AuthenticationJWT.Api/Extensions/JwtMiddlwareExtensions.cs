namespace AuthentificationJWT.Api.Extensions;

public static class JwtMiddlwareExtensions
{
    public static WebApplication UseJwtMiddlware(this WebApplication app)
    {
        app.UseMiddleware<JwtMiddleware>();
        return app;
    }
}
