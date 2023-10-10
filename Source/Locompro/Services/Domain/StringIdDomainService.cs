using Locompro.Data;
using Locompro.Repositories;

namespace Locompro.Services.Domain;

/// <summary>
/// Generic domain service for an application entity type with string ID.
/// </summary>
/// <typeparam name="T">Type of entity handled by service.</typeparam>
/// <typeparam name="R">Type of repository used by service.</typeparam>
public class StringIdDomainService<T, R> : AbstractDomainService<T, string, R>
    where T : class
    where R : StringIdRepository<T>
{
    protected StringIdDomainService(UnitOfWork unitOfWork, R repository, ILoggerFactory loggerFactory) 
        : base(unitOfWork, repository, loggerFactory)
    {
    }
    
    public async Task<IEnumerable<T>> GetByPartialId(string partialId)
    {
        await UnitOfWork.BeginTransaction();

        try
        {
            return await Repository.GetByPartialIdAsync(partialId);
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