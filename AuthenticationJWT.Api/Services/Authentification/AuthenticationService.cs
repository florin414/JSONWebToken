namespace AuthentificationJWT.Api.Services.Authentification;

public class AuthenticationService : IAuthenticationService
{
    private readonly ITokenService tokenService;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly TokenValidationParameters tokenValidationParameters;
    private readonly IRepository<User, string> userRepository;
    private readonly IRepository<RefreshToken, string> refreshTokenRepository;

    public AuthenticationService(
        ITokenService tokenService,
        IHttpContextAccessor httpContextAccessor,
        TokenValidationParameters tokenValidationParameters,
        IRepository<User, string> userRepository,
        IRepository<RefreshToken, string> refreshTokenRepository
    )
    {
        this.tokenService = Guard.ArgumentNotNull(tokenService, nameof(tokenService));
        this.httpContextAccessor = Guard.ArgumentNotNull(
            httpContextAccessor,
            nameof(httpContextAccessor)
        );
        this.tokenValidationParameters = Guard.ArgumentNotNull(
            tokenValidationParameters,
            nameof(tokenValidationParameters)
        );
        this.userRepository = Guard.ArgumentNotNull(userRepository, nameof(userRepository));
        this.refreshTokenRepository = Guard.ArgumentNotNull(
            refreshTokenRepository,
            nameof(refreshTokenRepository)
        );
    }

    public async Task<AuthenticationResponse> Authentication(
        UserAuthenticateRequest userAuthenticateRequest
    )
    {
        var user = this.userRepository
            .GetAll()
            .FirstOrDefault(
                x =>
                    x.UserName == userAuthenticateRequest.UserName
                    && x.Password == userAuthenticateRequest.Password
            );

        if (user == null)
        {
            return new AuthenticationResponse
            {
                Success = false,
                Errors = new[] { TokenErrors.Unauthorized }
            };
        }

        return await GenerateAuthenticationResultForUser(user);
    }

    public async Task<AuthenticationResponse> Registration(
        UserRegistretionRequest userRegistretionRequest
    )
    {
        if (userRegistretionRequest == null)
        {
            return new AuthenticationResponse
            {
                Success = false,
                Errors = new[] { TokenErrors.InvalidInput },
            };
        }
        var user = new User
        {
            UserName = userRegistretionRequest.UserName!,
            Password = userRegistretionRequest.Password!,
        };

        await this.userRepository.AddAsync(user);

        return await GenerateAuthenticationResultForUser(user);
    }

    public async Task<AuthenticationResponse> RefreshToken(RefreshTokenRequest refreshTokenRequest)
    {
        var validatedToken = TokenHelpers.GetClaimsPrincipalFromToken(
            refreshTokenRequest.Token,
            tokenValidationParameters
        );
        if (validatedToken == null)
        {
            return new AuthenticationResponse { Errors = new[] { TokenErrors.InvalidToken }, };
        }
        var expiryDateUnix = long.Parse(
            validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value
        );

        var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(
            expiryDateUnix
        );

        if (expiryDateTimeUtc > DateTime.UtcNow)
        {
            return new AuthenticationResponse
            {
                Errors = new[] { TokenErrors.HasNotExpiredYetToken },
            };
        }

        var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

        var storedRefreshToken = await this.refreshTokenRepository.GetById(
            refreshTokenRequest.RefreshToken
        );

        if (storedRefreshToken == null)
        {
            return new AuthenticationResponse
            {
                Errors = new[] { TokenErrors.NonExistentRefreshToken },
            };
        }

        if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
        {
            return new AuthenticationResponse
            {
                Errors = new[] { TokenErrors.ExpiredRefreshToken },
            };
        }

        if (storedRefreshToken.Invalidated)
        {
            return new AuthenticationResponse
            {
                Errors = new[] { TokenErrors.InvalidRefreshToken },
            };
        }

        if (storedRefreshToken.Used)
        {
            return new AuthenticationResponse { Errors = new[] { TokenErrors.RefreshTokenUsed }, };
        }

        if (storedRefreshToken.JwtId != jti)
        {
            return new AuthenticationResponse
            {
                Errors = new[] { TokenErrors.RefreshTokenUnmatchWithJwtId },
            };
        }

        storedRefreshToken.Used = true;
        await this.refreshTokenRepository.Update(storedRefreshToken);

        var validatedTokenClaimId = validatedToken.Claims.Single(x => x.Type == "id").Value;

        var user = await this.userRepository.GetById(validatedTokenClaimId);

        return await GenerateAuthenticationResultForUser(user);
    }

    private async Task<AuthenticationResponse> GenerateAuthenticationResultForUser(User user)
    {
        var createUserAuthToken = new CreateUserAuthToken
        {
            Id = user.Id,
            UserName = user.UserName!,
            Email = user.Email,
            Role = user.Role,
        };

        var token = tokenService.CreateToken(createUserAuthToken);

        var refreshToken = new RefreshToken
        {
            JwtId = token.id,
            UserId = user.Id,
            CreatedAt = DateTime.UtcNow,
            ExpiryDate = DateTime.UtcNow.AddDays(7), // create RefreshTokenOptions
            Used = false,
            Token = TokenHelpers.RandomString(35) + Guid.NewGuid(),
        };

        await this.refreshTokenRepository.AddAsync(refreshToken);

        var cookieOptions = new CookieOptions
        {
            Expires = DateTime.UtcNow.AddMinutes(30), // create CookieOptions
            Secure = true,
        };

        httpContextAccessor.HttpContext!.Response.Cookies.Append(
            nameof(token),
            token.signature,
            cookieOptions
        );
        httpContextAccessor.HttpContext!.Response.Cookies.Append(
            nameof(refreshToken),
            refreshToken.Token,
            cookieOptions
        );

        return new AuthenticationResponse
        {
            Success = true,
            Token = token.signature,
            RefreshToken = refreshToken.Token,
        };
    }
}
