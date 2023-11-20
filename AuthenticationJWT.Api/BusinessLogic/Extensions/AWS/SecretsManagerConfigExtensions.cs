namespace AuthenticationJWT.Api.BusinessLogic.Extensions.AWS;

public static class SecretsManagerConfigExtensions
{
    public static IConfigurationBuilder AddAmazonSecretsManager(
        this IConfigurationBuilder builder,
        IWebHostEnvironment environment
    )
    {
        var env = environment.EnvironmentName;
        var appName = environment.ApplicationName;

        builder.AddSecretsManager(configurator: options =>
        {
            options.SecretFilter = entry => entry.Name.StartsWith($"{env}_{appName}");
            options.KeyGenerator = (_, s) =>
                s.Replace($"{env}_{appName}_", string.Empty).Replace("__", ":");
        });

        return builder;
    }
}
