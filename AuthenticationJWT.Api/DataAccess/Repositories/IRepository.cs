﻿namespace AuthenticationJWT.Api.DataAccess.Repository;

public interface IRepository<TEntity, TId> where TEntity : class
{
    public IQueryable<TEntity> GetAll();
    public Task<TEntity> GetById(TId id);
    public Task AddAsync(TEntity entity);
    public Task UpdateAsync(TEntity entity);
    public Task RemoveAsync(TId id);
}
