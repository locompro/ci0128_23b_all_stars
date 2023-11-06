using Locompro.Common.Search;
using Locompro.Data;
using Locompro.Data.Repositories;
using Locompro.Models;

namespace Locompro.Services.Domain;

public class SearchDomainService : DomainService<Submission, SubmissionKey>, ISearchDomainService
{
    private readonly ISubmissionRepository _submissionRepository;

    public SearchDomainService(IUnitOfWork unitOfWork, ILoggerFactory loggerFactory) :
        base(unitOfWork, loggerFactory)
    {
        _submissionRepository = unitOfWork.GetSpecialRepository<ISubmissionRepository>();
    }
    
    public Task<IEnumerable<Submission>> GetSearchResults(SearchQueries searchQueries) =>
        _submissionRepository.GetSearchResults(searchQueries);
}