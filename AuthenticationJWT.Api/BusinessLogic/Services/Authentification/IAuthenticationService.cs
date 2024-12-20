﻿namespace AuthenticationJWT.Api.BusinessLogic.Services.Authentification;

public interface IAuthenticationService
{
    Task<AuthenticationResponse> Authentication(UserAuthenticateRequest userAuthenticateRequest);
    Task<AuthenticationResponse> Registration(UserRegistretionRequest userRegistretionRequest);
    Task<AuthenticationResponse> RefreshToken(RefreshTokenRequest refreshTokenRequest);
}
