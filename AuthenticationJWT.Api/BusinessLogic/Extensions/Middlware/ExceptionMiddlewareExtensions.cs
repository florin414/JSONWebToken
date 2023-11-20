namespace AuthenticationJWT.Api.BusinessLogic.Extensions.Middlware;

public static class ExceptionMiddlewareExtensions
{
    public static IServiceCollection AddExceptionMiddleware(this IServiceCollection services)
    {
        services.AddTransient<ExceptionMiddleware>();
        return services;
    }
}
