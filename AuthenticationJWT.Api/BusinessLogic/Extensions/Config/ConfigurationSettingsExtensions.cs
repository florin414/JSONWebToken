namespace AuthenticationJWT.Api.BusinessLogic.Extensions.Config;

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
