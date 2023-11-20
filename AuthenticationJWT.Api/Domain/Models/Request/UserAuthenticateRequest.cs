namespace AuthenticationJWT.Api.Domain.Models.Request;

public class UserAuthenticateRequest
{
    [RegularExpression("^[A-Za-z][A-Za-z0-9_]{7,29}$")]
    public required string UserName { get; set; } = string.Empty;

    [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")]
    public required string Password { get; set; } = string.Empty;
}
