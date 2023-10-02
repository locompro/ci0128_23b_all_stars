namespace Locompro.Services;

/// <summary>
/// Interface representing domain services.
/// </summary>
/// <typeparam name="TEntity">Type of entity handled by service.</typeparam>
/// <typeparam name="TKey">Type of key used by entity.</typeparam>
public interface IDomainService<TEntity, in TKey>
{
    Task<TEntity> Get(TKey id);

    Task<IEnumerable<TEntity>> GetAll();

    Task Add(TEntity entity);

    Task Update(TEntity entity);

    Task Delete(TKey id);
}