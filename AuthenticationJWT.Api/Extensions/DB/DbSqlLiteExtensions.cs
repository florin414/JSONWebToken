namespace AuthenticationJWT.Api.Extensions.DB;

public static class DbSqlLiteExtensions
{
    public static IServiceCollection AddDbSqlLite(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddScoped<IRepository<RefreshToken, string>, RefreshTokenRepository>();
        services.AddScoped<IRepository<User, string>, UserRepository>();

        var connectionString = configuration.GetConnectionString("AuthenticationJWTConnection");
        services.AddDbContext<ApiDbContext>(options => options.UseSqlite(connectionString));

        services
            .AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApiDbContext>();

        return services;
    }

    public static IServiceProvider DbSqlLiteMigrate(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dataContext = scope.ServiceProvider.GetRequiredService<ApiDbContext>();
        dataContext.Database.Migrate();

        return serviceProvider;
    }
}
