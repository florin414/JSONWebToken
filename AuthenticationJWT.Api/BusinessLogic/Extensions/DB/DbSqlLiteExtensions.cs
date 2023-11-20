namespace AuthenticationJWT.Api.BusinessLogic.Extensions.DB;

public static class DbSqlLiteExtensions
{
    public static IServiceCollection AddDbSqlLite(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var connectionString = configuration.GetConnectionString("SQLiteDbConnection");
        services.AddDbContext<ApiDbContext>(options => options.UseSqlite(connectionString));

        services.AddScoped<IRepository<RefreshToken, string>, RefreshTokenRepository>();
        services.AddScoped<IRepository<User, string>, UserRepository>();
        services.AddScoped<IRepository<Product, int>, ProductRepository>();

        services.AddDefaultIdentity<User>(options =>
        options.SignIn.RequireConfirmedAccount = false)
            .AddEntityFrameworkStores<ApiDbContext>()
            .AddDefaultTokenProviders();

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
