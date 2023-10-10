using Locompro.Data;
using Locompro.Models;
using Locompro.Repositories;

namespace Locompro.Services.Domain;

public class SubmissionService : AbstractDomainService<Submission, SubmissionKey, SubmissionRepository>
{
    public SubmissionService(UnitOfWork unitOfWork, SubmissionRepository repository, ILoggerFactory loggerFactory)
        : base(unitOfWork, repository, loggerFactory)
    {
    }
}