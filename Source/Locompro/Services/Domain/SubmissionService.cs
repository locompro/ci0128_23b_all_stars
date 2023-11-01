using Locompro.Data;
using Locompro.Models;
using Locompro.Data.Repositories;
using Locompro.Repositories.Utilities;

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
    public async Task<IEnumerable<Submission>> GetByProductName(string productName)
    {
        return await _submissionRepository.GetByProductNameAsync(productName);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Submission>> GetByProductModel(string productModel)
    {
        return await _submissionRepository.GetByProductModelAsync(productModel);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Submission>> GetByBrand(string brandName)
    {
        return await _submissionRepository.GetByBrandAsync(brandName);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Submission>> GetByCantonAndProvince(string canton, string province)
    {
        return await _submissionRepository.GetByCantonAsync(canton, province);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Submission>> GetByCanton(string canton, string province)
    {
        return await _submissionRepository.GetByCantonAsync(canton, province);
    }
}