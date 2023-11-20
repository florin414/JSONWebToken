namespace AuthenticationJWT.Api.BusinessLogic.Services.Authentification;

public class AuthenticationService : IAuthenticationService
{
    private readonly ITokenService tokenService;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly IRepository<User, string> userRepository;
    private readonly IRepository<RefreshToken, string> refreshTokenRepository;
    private readonly IOptions<JwtOptions> jwtOptions;
    private UserManager<User> userManager;

    public AuthenticationService(
        ITokenService tokenService,
        IHttpContextAccessor httpContextAccessor,
        IRepository<User, string> userRepository,
        IRepository<RefreshToken, string> refreshTokenRepository,
        IOptions<JwtOptions> jwtOptions,
        UserManager<User> userManager
    )
    {
        this.tokenService = Guard.ArgumentNotNull(tokenService, nameof(tokenService));
        this.httpContextAccessor = Guard.ArgumentNotNull(
            httpContextAccessor,
            nameof(httpContextAccessor)
        );
        this.userRepository = Guard.ArgumentNotNull(userRepository, nameof(userRepository));
        this.refreshTokenRepository = Guard.ArgumentNotNull(
            refreshTokenRepository,
            nameof(refreshTokenRepository)
        );
        this.jwtOptions = Guard.ArgumentNotNull(jwtOptions, nameof(jwtOptions));
        this.userManager = Guard.ArgumentNotNull(userManager, nameof(userManager));
    }

    public async Task<AuthenticationResponse> Authentication(
        UserAuthenticateRequest userAuthenticateRequest
    )
    {
        var user = userRepository
            .GetAll()
            .ToList()
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
        var user = new User
        {
            UserName = userRegistretionRequest.UserName!,
            Password = userRegistretionRequest.Password!,
        };

        var result = await userManager.CreateAsync(user, userRegistretionRequest.Password);

        if (result.Succeeded)
        {
            return await GenerateAuthenticationResultForUser(user);
        }

        var errors = result.Errors.Select(x => x.Description);

        return new AuthenticationResponse
        {
            Success = false,
            Errors = errors
        };
    }

    public async Task<AuthenticationResponse> RefreshToken(RefreshTokenRequest refreshTokenRequest)
    {
        var validatedToken = TokenHelpers.GetPrincipalFromExpiredToken(
            refreshTokenRequest.Token,
            jwtOptions
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

        var storedRefreshToken = await refreshTokenRepository.GetById(
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
        await refreshTokenRepository.UpdateAsync(storedRefreshToken);

        var validatedTokenClaimId = validatedToken.Claims.Single(x => x.Type == "id").Value;

        var user = await userRepository.GetById(validatedTokenClaimId);

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
            ExpiryDate = DateTime.UtcNow.AddDays(7),
            Used = false,
            Token = TokenHelpers.RandomString(35) + Guid.NewGuid(),
        };

        await refreshTokenRepository.AddAsync(refreshToken);

        var cookieOptions = new CookieOptions
        {
            Expires = refreshToken.ExpiryDate,
            Secure = true,
            HttpOnly = true,
            SameSite = SameSiteMode.Strict
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
