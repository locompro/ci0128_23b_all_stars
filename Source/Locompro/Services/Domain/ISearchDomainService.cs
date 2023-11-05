using Locompro.Common.Search;
using Locompro.Models;
using Locompro.Models.Entities;

namespace Locompro.Services.Domain;

public interface ISearchDomainService
{
    Task<IEnumerable<Submission>> GetSearchResults(SearchQueries searchQueries);
}