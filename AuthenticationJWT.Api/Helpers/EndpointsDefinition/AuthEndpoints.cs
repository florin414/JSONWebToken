namespace AuthentificationJWT.Api.Helpers.EndpointsDefinition;

public static class AuthEndpoints
{
    public const string Tag = "Auth";
    private const string BaseUrl = "/auth";
    public const string Authentication = BaseUrl + "/login";
    public const string Registration = BaseUrl + "/registration";
    public const string RefreshToken = BaseUrl + "/refreshToken";
}
