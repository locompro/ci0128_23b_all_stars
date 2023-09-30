using Locompro.Repositories;

namespace Locompro.Services;

/// <summary>
/// Abstract class representing domain services.
/// </summary>
/// <typeparam name="TEntity">Type of entity handled by service.</typeparam>
/// <typeparam name="TKey">Type of key used by entity.</typeparam>
/// <typeparam name="TRepo">Type of repository used by service.</typeparam>
public abstract class AbstractDomainService<TEntity, TKey, TRepo> : AbstractService, IDomainService<TEntity, TKey>
    where TEntity : class
    where TRepo : IRepository<TEntity, TKey>
{
    protected readonly TRepo Repository;

    protected AbstractDomainService(UnitOfWork unitOfWork, TRepo repository) : base(unitOfWork)
    {
        Repository = repository;
    }

    public async Task<TEntity> Get(TKey id)
    {
        await UnitOfWork.BeginTransaction();

        try
        {
            return await Repository.GetByIdAsync(id);
        }
        catch (Exception)
        {
            await UnitOfWork.Rollback();
            throw;
        }
        finally
        {
            await UnitOfWork.Commit();
        }
    }

    public async Task<IEnumerable<TEntity>> GetAll()
    {
        await UnitOfWork.BeginTransaction();

        try
        {
            return await Repository.GetAllAsync();
        }
        catch (Exception)
        {
            await UnitOfWork.Rollback();
            throw;
        }
        finally
        {
            await UnitOfWork.Commit();
        }
    }

    public async Task Add(TEntity entity)
    {
        await UnitOfWork.BeginTransaction();

        try
        {
            await Repository.AddAsync(entity);
        }
        catch (Exception)
        {
            await UnitOfWork.Rollback();
            throw;
        }
        finally
        {
            await UnitOfWork.Commit();
        }
    }

    public async Task Update(TEntity entity)
    {
        await UnitOfWork.BeginTransaction();

        try
        {
            await Repository.UpdateAsync(entity);
        }
        catch (Exception)
        {
            await UnitOfWork.Rollback();
            throw;
        }
        finally
        {
            await UnitOfWork.Commit();
        }
    }

    public async Task Delete(TKey id)
    {
        await UnitOfWork.BeginTransaction();

        try
        {
            await Repository.DeleteAsync(id);
        }
        catch (Exception)
        {
            await UnitOfWork.Rollback();
            throw;
        }
        finally
        {
            await UnitOfWork.Commit();
        }
    }
}