using Locompro.Models;
using Locompro.SearchQueryConstruction;

namespace Locompro.Services.Domain;

public interface ISearchDomainService
{
    Task<IEnumerable<Submission>> GetSearchResults(SearchQueries searchQueries);
}