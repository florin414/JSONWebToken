namespace AuthenticationJWT.Api.BusinessLogic.Extensions.Middlware;

public static class ErrorHandlerMiddlewareExtensions
{
    public static WebApplication UseErrorHandlerMiddlware(this WebApplication app)
    {
        app.UseMiddleware<ErrorHandlerMiddleware>();
        return app;
    }
}
