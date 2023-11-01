using Locompro.Models;
using Locompro.Repositories.Utilities;

namespace Locompro.Services.Domain;

public interface ISearchDomainService
{
    Task<IEnumerable<Submission>> GetSearchResults(SearchQueries searchQueries);
}