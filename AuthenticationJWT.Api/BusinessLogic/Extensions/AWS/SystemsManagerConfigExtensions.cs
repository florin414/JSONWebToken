namespace AuthenticationJWT.Api.BusinessLogic.Extensions.AWS;

public static class SystemsManagerConfigExtensions
{
    public static IConfigurationBuilder AddAmazonSystemsManager(
        this IConfigurationBuilder builder,
        IWebHostEnvironment environment
    )
    {
        var env = environment.EnvironmentName.ToLower();
        builder.AddSystemsManager($"/{env}/spoonacularfood-api");

        return builder;
    }
}
