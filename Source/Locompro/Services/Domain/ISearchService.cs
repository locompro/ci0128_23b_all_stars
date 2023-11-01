using Locompro.Models;
using Locompro.Repositories.Utilities.Interfaces;

namespace Locompro.Services.Domain;

public interface ISearchService
{
    Task<List<Item>> GetSearchResults(List<ISearchCriterion> unfilteredSearchCriteria);
}