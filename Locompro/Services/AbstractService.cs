using Locompro.Repositories;

namespace Locompro.Services;

/// <summary>
/// Abstract class representing services.
/// </summary>
public class AbstractService
{
    protected readonly UnitOfWork UnitOfWork;

    protected AbstractService(UnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork;
    }
}