namespace AuthenticationJWT.Api.DataAccess.Repository;

public class UserRepository : IRepository<User, string>
{
    protected ApiDbContext DbContext { get; set; }

    public UserRepository(ApiDbContext dbContext)
    {
        DbContext = Guard.ArgumentNotNull(dbContext, nameof(dbContext));
    }

    public async Task AddAsync(User entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        await this.DbContext.AddAsync(entity);
        await this.DbContext.SaveChangesAsync();
    }

    public async Task RemoveAsync(string id)
    {
        var entity = await DbContext.Set<User>().FindAsync(id);
        ArgumentNullException.ThrowIfNull(entity);
        DbContext.Remove(entity);
        await this.DbContext.SaveChangesAsync();
    }

    public IQueryable<User> GetAll()
    {
        return this.DbContext.Set<User>();
    }

    public async Task<User> GetById(string id)
    {
        var entity = await DbContext.FindAsync<User>(id);
        ArgumentNullException.ThrowIfNull(entity);
        return entity;
    }

    public async Task Update(User entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        this.DbContext.Update(entity);
        await this.DbContext.SaveChangesAsync();
    }
}
