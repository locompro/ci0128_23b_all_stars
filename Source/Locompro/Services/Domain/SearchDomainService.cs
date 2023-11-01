using Locompro.Data;
using Locompro.Data.Repositories;
using Locompro.Models;
using Locompro.Repositories.Utilities;

namespace Locompro.Services.Domain;

public class SearchDomainService : DomainService<Submission, SubmissionKey>, ISearchDomainService
{
    private readonly ISubmissionRepository _submissionRepository;

    public SearchDomainService(IUnitOfWork unitOfWork, ILoggerFactory loggerFactory) :
        base(unitOfWork, loggerFactory)
    {
        _submissionRepository = unitOfWork.GetRepository<ISubmissionRepository>();
    }
    
    public Task<IEnumerable<Submission>> GetSearchResults(SearchQueries searchQueries) =>
        _submissionRepository.GetSearchResults(searchQueries);
}