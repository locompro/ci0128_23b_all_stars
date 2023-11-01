using Locompro.Common.Search;
using Locompro.Models;

namespace Locompro.Services.Domain;

public interface ISearchDomainService
{
    Task<IEnumerable<Submission>> GetSearchResults(SearchQueries searchQueries);
}