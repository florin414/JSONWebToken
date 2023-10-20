namespace AuthentificationJWT.Api.Extensions.Auth;

public static class AuthEndpointsApiExtensions
{
    public static WebApplication MapAuthEndpointsApi(this WebApplication app)
    {
        app.MapPost(AuthEndpoints.Authentication, AuthenticationUserHandlerAsync)
            .WithTags(AuthEndpoints.Tag)
            .AllowAnonymous();

        app.MapPost(AuthEndpoints.Registration, RegistrationUserHandlerAsync)
            .WithTags(AuthEndpoints.Tag)
            .AllowAnonymous();

        app.MapPost(AuthEndpoints.RefreshToken, RefreshTokenHandlerAsync)
            .WithTags(AuthEndpoints.Tag)
            .AllowAnonymous();

        return app;
    }

    private static async Task<IResult> AuthenticationUserHandlerAsync(
        IAuthenticationService userAuthentificationService,
        UserAuthenticateRequest userAuthenticateRequest
    )
    {
        var authentificationResponse = await userAuthentificationService.Authentication(
            userAuthenticateRequest
        );

        return authentificationResponse.Success
            ? Results.Ok(
                new AuthSuccessResponse
                {
                    Token = authentificationResponse.Token!,
                    RefreshToken = authentificationResponse.RefreshToken
                }
            )
            : Results.BadRequest(authentificationResponse.Errors);
    }

    private static async Task<IResult> RegistrationUserHandlerAsync(
        IAuthenticationService userAuthentificationService,
        UserRegistretionRequest registrationUserRegistretionRequest
    )
    {
        var authentificationResponse = await userAuthentificationService.Registration(
            registrationUserRegistretionRequest
        );

        return authentificationResponse.Success
            ? Results.Ok(
                new AuthSuccessResponse
                {
                    Token = authentificationResponse.Token!,
                    RefreshToken = authentificationResponse.RefreshToken
                }
            )
            : Results.BadRequest(authentificationResponse.Errors);
    }

    private static async Task<IResult> RefreshTokenHandlerAsync(
        IAuthenticationService userAuthentificationService,
        RefreshTokenRequest refreshTokenRequest
    )
    {
        var authentificationResponse = await userAuthentificationService.RefreshToken(
            refreshTokenRequest
        );

        return authentificationResponse.Success
            ? Results.Ok(
                new AuthSuccessResponse
                {
                    Token = authentificationResponse.Token!,
                    RefreshToken = authentificationResponse.RefreshToken
                }
            )
            : Results.BadRequest(authentificationResponse.Errors);
    }
}
