namespace AuthenticationJWT.Api.Middlewares;

public class AuthInterceptor : IMiddleware
{
    public AuthInterceptor() {}

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var requiresAuthorization = context.GetEndpoint()?.Metadata.GetMetadata<AuthorizeAttribute>() != null;

        if(requiresAuthorization)
        {
            var token = context.Request.Cookies["token"];
            if (!string.IsNullOrEmpty(token))
            {
                context.Request.Headers.Append("Authorization", $"Bearer {token}");
            }
        }

        await next(context);
    }
}
