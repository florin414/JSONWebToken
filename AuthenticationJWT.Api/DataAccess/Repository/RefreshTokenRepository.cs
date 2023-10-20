namespace AuthenticationJWT.Api.DataAccess.Repository;

public class RefreshTokenRepository : IRepository<RefreshToken, string>
{
    protected ApiDbContext DbContext { get; set; }

    public RefreshTokenRepository(ApiDbContext dbContext)
    {
        DbContext = Guard.ArgumentNotNull(dbContext, nameof(dbContext));
    }

    public async Task AddAsync(RefreshToken entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        await this.DbContext.AddAsync(entity);
        await this.DbContext.SaveChangesAsync();
    }

    public async Task RemoveAsync(string id)
    {
        var entity = await DbContext.Set<RefreshToken>().FindAsync(id);
        ArgumentNullException.ThrowIfNull(entity);
        DbContext.Remove(entity);
        await this.DbContext.SaveChangesAsync();
    }

    public IQueryable<RefreshToken> GetAll()
    {
        return this.DbContext.Set<RefreshToken>();
    }

    public async Task<RefreshToken> GetById(string id)
    {
        var entity = await DbContext.FindAsync<RefreshToken>(id);
        ArgumentNullException.ThrowIfNull(entity);
        return entity;
    }

    public async Task Update(RefreshToken entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        this.DbContext.Update(entity);
        await this.DbContext.SaveChangesAsync();
    }
}
