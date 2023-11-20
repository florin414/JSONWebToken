namespace AuthenticationJWT.Api.Domain.Helpers;

public static class TokenErrors
{
    public const string Unauthorized = "401 Unauthorized";
    public const string InvalidInput = "Invalid Input";
    public const string InvalidToken = "Invalid Token";
    public const string HasNotExpiredYetToken = "This token hasn't expired yet";
    public const string NonExistentRefreshToken = "This refresh token does not exist";
    public const string ExpiredRefreshToken = "This refresh token has' expired";
    public const string InvalidRefreshToken = "Invalid Refresh Token";
    public const string RefreshTokenUsed = "This refresh token has been used";
    public const string RefreshTokenUnmatchWithJwtId = "This refresh does not match this JWT";
}
