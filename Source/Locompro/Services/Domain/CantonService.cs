using Locompro.Data;
using Locompro.Models;
using Locompro.Repositories;

namespace Locompro.Services.Domain;

public class CantonService : AbstractDomainService<Canton, string, CantonRepository>
{
    public CantonService(UnitOfWork unitOfWork, CantonRepository repository, ILoggerFactory loggerFactory) : base(unitOfWork, repository, loggerFactory)
    {
    }
    
    public async Task<Canton> Get(string country, string province, string canton)
    {
        await UnitOfWork.BeginTransaction();

        try
        {
            return await Repository.GetByIdAsync(country, province, canton);
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