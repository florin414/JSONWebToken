namespace AuthenticationJWT.Api.BusinessLogic.Services.Token;

public interface ITokenService
{
    (string signature, string id) CreateToken(CreateUserAuthToken createUserAuthToken);
}
