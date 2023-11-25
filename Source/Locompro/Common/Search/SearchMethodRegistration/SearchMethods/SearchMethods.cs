using System.Linq.Expressions;
using Locompro.Common.Search.SearchFilters;

namespace Locompro.Common.Search.SearchMethodRegistration.SearchMethods;


/// <summary>
/// Singleton class that is used to store all the search strategies for a given TSearchResult
/// Must be inherited by a class that implements the InitializeSearchMethods method
/// </summary>
/// <typeparam name="TSearchResult"> Type of result that will be searched</typeparam>
/// <typeparam name="TSearchMethods"> Type of derived SearchMethods class,
/// primarily used for singleton creation</typeparam>
public abstract class SearchMethods<TSearchResult, TSearchMethods> : ISearchMethods
    where TSearchMethods : SearchMethods<TSearchResult, TSearchMethods>, new()
{
    private readonly Dictionary<SearchParameterTypes, ISearchParam> _searchParameters;

    private readonly Dictionary<SearchParameterTypes, ISearchFilterParam> _searchFilterParameters;
    
    private static TSearchMethods _instance;
    
    /// <summary>
    /// Returns the singleton instance of the search methods
    /// </summary>
    /// <returns> Instance of the specific derived class </returns>
    public static ISearchMethods GetInstance()
    {
        if (_instance != null) return _instance;
        
        _instance = new TSearchMethods();
        _instance.InitializeSearchMethods();
        return _instance;
    }

    /// <inheritdoc />
    public ISearchParam GetSearchMethodByName(SearchParameterTypes parameterName)
    {
        _searchParameters.TryGetValue(parameterName, out var searchParameter);
        return searchParameter;
    }

    /// <inheritdoc />
    public bool Contains(SearchParameterTypes parameterName)
    {
        return _searchParameters.ContainsKey(parameterName);
    }
    
    /// <inheritdoc />
    public ISearchFilterParam GetSearchFilterByName(SearchParameterTypes parameterName)
    {
        _searchFilterParameters.TryGetValue(parameterName, out var searchParameter);
        return searchParameter;
    }
    
    /// <inheritdoc />
    public bool ContainsSearchFilter(SearchParameterTypes parameterName)
    {
        return _searchFilterParameters.ContainsKey(parameterName);
    }
    
    /// <summary>
    ///     Constructor
    /// </summary>
    protected SearchMethods()
    {
        _searchFilterParameters = new Dictionary<SearchParameterTypes, ISearchFilterParam>();
        _searchParameters = new Dictionary<SearchParameterTypes, ISearchParam>();
    }
    
    /// <summary>
    ///     Adds a new search parameter, strategy or way to search for a given TSearchResult
    /// </summary>
    /// <param name="parameterName"> the name to find which strategy to use </param>
    /// <param name="searchQuery"> the strategy of how to find a given TSearchResult </param>
    /// <param name="activationQualifier">
    ///     a function used to determine whether the search is to be performed or not. e.g. if
    ///     string is not null then search
    /// </param>
    protected void AddSearchParameter<TSearchParameter>(SearchParameterTypes parameterName,
        Expression<Func<TSearchResult, TSearchParameter, bool>> searchQuery, Func<TSearchParameter, bool> activationQualifier)
    {
        _searchParameters.Add(
            parameterName,
            new SearchParam
            {
                SearchQuery = new SearchQuery<TSearchResult, TSearchParameter>(searchQuery),
                ActivationQualifier = new ActivationQualifier<TSearchParameter>(activationQualifier)
            }
        );
    }
    
    /// <summary>
    ///     Adds a new search filter, strategy or way to filter a given TSearchResult
    /// </summary>
    /// <param name="searchParameterType"> the name to find which filter to apply </param>
    /// <param name="searchQuery"> the strategy of how to filter for a given TSearchResult</param>
    /// <param name="activationQualifier">
    ///     a function used to determine whether the filter is to be performed or not. e.g. if
    ///     string is not null then search
    /// </param>
    /// <typeparam name="TSearchParameter"> type of parameter for which the search is being made</typeparam>
    protected void AddSearchFilter<TSearchParameter>(
        SearchParameterTypes searchParameterType,
        Func<TSearchResult, TSearchParameter, bool> searchQuery,
        Func<TSearchParameter, bool> activationQualifier)
    {
        _searchFilterParameters.Add(
            searchParameterType,
            new SearchFilterParam(
                new SearchFilterQuery<TSearchResult, TSearchParameter>(searchQuery),
                new ActivationQualifier<TSearchParameter>(activationQualifier)
                )
            );
    }

    /// <summary>
    /// Initializes the search methods, must be overridden
    /// Should inside call AddSearchParameter for each search method that is to be added
    /// </summary>
    protected abstract void InitializeSearchMethods();
}