namespace AuthenticationJWT.Api.DataAccess.Repository;

public class ProductRepository : IRepository<Product, int>
{
    protected ApiDbContext DbContext { get; set; }

    private readonly string connectionString;

    public ProductRepository(ApiDbContext dbContext, IConfiguration configuration)
    {
        DbContext = Guard.ArgumentNotNull(dbContext, nameof(dbContext));
        ArgumentNullException.ThrowIfNull(configuration);
        connectionString = configuration.GetConnectionString("SQLiteDbConnection")!;
    }

    public async Task AddAsync(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);
        await DbContext.AddAsync(product);
        await DbContext.SaveChangesAsync();
    }

    public IQueryable<Product> GetAll()
    {
        using var connection = new SqliteConnection(connectionString);

        var getAllProductsQuery = "select * from Product";
        var product = connection.Query<Product>(getAllProductsQuery);
        var castProductAsQueryable = product.AsQueryable();

        return castProductAsQueryable;
    }

    public async Task<Product> GetById(int id)
    {
        using var connection = new SqliteConnection(connectionString);

        var getByIdProductQuery = "select * from Product where Id = @Id";
        var product = await connection.QueryFirstOrDefaultAsync<Product>(
            getByIdProductQuery,
            new { Id = id }
        );

        return product;
    }

    public async Task RemoveAsync(int id)
    {
        using var connection = new SqliteConnection(connectionString);

        var removeProductQuery = "delete from RefreshTokens where Id = @RefreshTokenId";
        await connection.ExecuteAsync(removeProductQuery, new { RefreshTokenId = id });
    }

    public async Task UpdateAsync(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);
        DbContext.Update(product);
        await DbContext.SaveChangesAsync();
    }
}
