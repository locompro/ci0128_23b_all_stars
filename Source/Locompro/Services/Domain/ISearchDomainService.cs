using Locompro.Common.Search;
using Locompro.Models.Entities;

namespace Locompro.Services.Domain;

public interface ISearchDomainService
{
    Task<IEnumerable<Submission>> GetSearchResults(ISearchQueries searchQueries);
}