namespace AuthenticationJWT.Api.DataAccess;

public class ApiDbContext : IdentityDbContext<User, IdentityRole<string>, string>
{
    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.SeedUsers();
    }
}
