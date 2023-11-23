using System.Linq.Expressions;

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
    private readonly Dictionary<SearchParameterTypes, SearchParam> _searchParameters;
    
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
    public SearchParam GetSearchMethodByName(SearchParameterTypes parameterName)
    {
        _searchParameters.TryGetValue(parameterName, out var searchParameter);
        return searchParameter;
    }

    /// <inheritdoc />
    public bool Contains(SearchParameterTypes parameterName)
    {
        return _searchParameters.ContainsKey(parameterName);
    }
    
    /// <summary>
    ///     Constructor
    /// </summary>
    protected SearchMethods()
    {
        _searchParameters = new Dictionary<SearchParameterTypes, SearchParam>();
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
    /// Initializes the search methods, must be overridden
    /// Should inside call AddSearchParameter for each search method that is to be added
    /// </summary>
    protected abstract void InitializeSearchMethods();
}