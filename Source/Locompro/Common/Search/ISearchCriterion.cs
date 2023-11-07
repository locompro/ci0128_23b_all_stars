using Locompro.Common.Search.SearchMethodRegistration;

namespace Locompro.Common.Search;

/// <summary>
///     Interface for SearchCriterion to implement as generic class
/// </summary>
public interface ISearchCriterion
{
    public SearchParameterTypes ParameterName { get; init; }

    /// <summary>
    ///     gets the internal search value declared by the generic class
    /// </summary>
    /// <returns></returns>
    public dynamic GetSearchValue();
}