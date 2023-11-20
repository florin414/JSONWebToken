namespace AuthenticationJWT.Api.DataAccess.Repository;

public class UserRepository : IRepository<User, string>
{
    protected ApiDbContext DbContext { get; set; }
    private readonly string connectionString;

    public UserRepository(ApiDbContext dbContext, IConfiguration configuration)
    {
        DbContext = Guard.ArgumentNotNull(dbContext, nameof(dbContext));
        ArgumentNullException.ThrowIfNull(configuration);
        connectionString = configuration.GetConnectionString("SQLiteDbConnection")!;
    }

    public async Task AddAsync(User user)
    {
        ArgumentNullException.ThrowIfNull(user);
        await DbContext.AddAsync(user);
        await DbContext.SaveChangesAsync();
    }

    public async Task RemoveAsync(string id)
    {
        using var connection = new SqliteConnection(connectionString);

        var removeUserQuery = "delete from AspNetUsers where Id = @UserId";
        await connection.ExecuteAsync(removeUserQuery, new { UserId = id });
    }

    public IQueryable<User> GetAll()
    {
        using var connection = new SqliteConnection(connectionString);

        var getAllUsersQuery = "select * from AspNetUsers";
        var users = connection.Query<User>(getAllUsersQuery);
        var castUserAsQueryable = users.AsQueryable();

        return castUserAsQueryable;
    }

    public async Task<User> GetById(string id)
    {
        using var connection = new SqliteConnection(connectionString);

        var getByIdUserQuery = "select * from AspNetUsers where id = @Id";
        var user = await connection.QueryFirstOrDefaultAsync<User>(
            getByIdUserQuery,
            new { Id = id }
        );

        return user;
    }

    public async Task UpdateAsync(User user)
    {
        ArgumentNullException.ThrowIfNull(user);
        DbContext.Update(user);
        await DbContext.SaveChangesAsync();
    }
}
