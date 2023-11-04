using Locompro.Common.Search;
using Locompro.Data;
using Locompro.Models;
using Locompro.Data.Repositories;

namespace Locompro.Services.Domain;

public class SubmissionService : DomainService<Submission, SubmissionKey>, ISubmissionService
{
    private readonly ISubmissionRepository _submissionRepository;
    
    public SubmissionService(IUnitOfWork unitOfWork, ILoggerFactory loggerFactory) : base(unitOfWork, loggerFactory)
    {
        _submissionRepository = UnitOfWork.GetRepository<ISubmissionRepository>();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Submission>> GetSearchResults(SearchQueries searchQueries)
    {
        return await _submissionRepository.GetSearchResults(searchQueries);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Submission>> GetItemSubmissions(string storeName, string productName)
    {
        return await _submissionRepository.GetItemSubmissions(storeName, productName);
    }
}