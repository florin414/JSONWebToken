namespace AuthentificationJWT.Api.Services.Token;

public interface ITokenService
{
    (string signature, string id) CreateToken(CreateUserAuthToken createUserAuthToken);
}
