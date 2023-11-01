using Locompro.Common.Search.Interfaces;
using Locompro.Models;

namespace Locompro.Services.Domain;

public interface ISearchService
{
    Task<List<Item>> GetSearchResults(List<ISearchCriterion> unfilteredSearchCriteria);
}