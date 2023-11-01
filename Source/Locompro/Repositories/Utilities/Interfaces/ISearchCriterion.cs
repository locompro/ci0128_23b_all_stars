namespace Locompro.SearchQueryConstruction;

/// <summary>
/// Interface for SearchCriterion to implement as generic class 
/// </summary>
public interface ISearchCriterion
{
    public SearchParameterTypes ParameterName { get; init; }

    public dynamic GetSearchValue();
}