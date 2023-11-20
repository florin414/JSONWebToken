namespace AuthenticationJWT.Api.Domain.Helpers.EndpointsDefinition;

public static class AuthEndpoints
{
    private const string BaseUrl = "/auth";
    public const string Tag = "Auth";
    public const string Authentication = BaseUrl + "/login";
    public const string Registration = BaseUrl + "/registration";
    public const string RefreshToken = BaseUrl + "/refreshToken";
}
