using AuthentificationJWT.Api.Models.Config;

namespace AuthenticationJWT.Api.Extensions;

public static class ConfigurationSettingsExtensions
{
    public static IServiceCollection AddConfig(
        this IServiceCollection services,
        IConfiguration config
    )
    {
        services.Configure<SpoonacularFoodApiSettings>(
            config.GetSection(SpoonacularFoodApiSettings.SpoonacularFoodApi)
        );

        services.Configure<JwtOptions>(config.GetSection(JwtOptions.JwtSettings));

        return services;
    }
}
