namespace AuthenticationJWT.Api.DataAccess.Repository;

public class RefreshTokenRepository : IRepository<RefreshToken, string>
{
    protected ApiDbContext DbContext { get; set; }

    private readonly string connectionString;

    public RefreshTokenRepository(ApiDbContext dbContext, IConfiguration configuration)
    {
        DbContext = Guard.ArgumentNotNull(dbContext, nameof(dbContext));
        ArgumentNullException.ThrowIfNull(configuration);
        connectionString = configuration.GetConnectionString("SQLiteDbConnection")!;
    }

    public async Task AddAsync(RefreshToken refreshToken)
    {
        ArgumentNullException.ThrowIfNull(refreshToken);
        await DbContext.AddAsync(refreshToken);
        await DbContext.SaveChangesAsync();
    }

    public async Task RemoveAsync(string id)
    {
        using var connection = new SqliteConnection(connectionString);

        var removeRefreshTokenQuery = "delete from RefreshTokens where Id = @RefreshTokenId";
        await connection.ExecuteAsync(removeRefreshTokenQuery, new { RefreshTokenId = id });
    }

    public IQueryable<RefreshToken> GetAll()
    {
        using var connection = new SqliteConnection(connectionString);

        var getAllRefreshTokenQuery = "select * from RefreshTokens";
        var refreshTokens = connection.Query<RefreshToken>(getAllRefreshTokenQuery);
        var castRefreshTokensAsQueryable = refreshTokens.AsQueryable();

        return castRefreshTokensAsQueryable;
    }

    public async Task<RefreshToken> GetById(string id)
    {
        using var connection = new SqliteConnection(connectionString);

        var getByIdRefreshTokenQuery = "select * from RefreshTokens where Token = @Id";
        var refreshTokens = await connection.QueryFirstOrDefaultAsync<RefreshToken>(
            getByIdRefreshTokenQuery,
            new { Id = id }
        );

        return refreshTokens;
    }

    public async Task UpdateAsync(RefreshToken refreshToken)
    {
        ArgumentNullException.ThrowIfNull(refreshToken);
        DbContext.Update(refreshToken);
        await DbContext.SaveChangesAsync();
    }
}
