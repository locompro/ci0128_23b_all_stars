namespace Locompro.Services
{
    /// <summary>
    /// Interface representing domain services.
    /// </summary>
    /// <typeparam name="T">Type of entity handled by service.</typeparam>
    /// <typeparam name="I">Type of key used by entity.</typeparam>
    public interface IDomainService<T, I>
    {
        Task<T> Get(I id);

        Task<IEnumerable<T>> GetAll();

        Task Add(T entity);

        Task Update(T entity);

        Task Delete(I id);
    }
}
