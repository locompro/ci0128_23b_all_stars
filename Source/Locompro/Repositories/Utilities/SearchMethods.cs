using System.Linq.Expressions;
using Locompro.Models;

namespace Locompro.Repositories.Utilities;

/// <summary>
/// Singleton class that is used to store all the search strategies
/// </summary>
public class SearchMethods
{
    private static SearchMethods _instance;
    private readonly Dictionary<SearchParam.SearchParameterTypes, SearchParam> _searchParameters;
    
    /// <summary>
    /// Singleton private constructor
    /// </summary>
    private SearchMethods()
    {
        this._searchParameters = new Dictionary<SearchParam.SearchParameterTypes, SearchParam>();
        this.AddAllSearchParameters();
    }
    
    /// <summary>
    /// returns instance of the singleton class
    /// </summary>
    public static SearchMethods GetInstance => SearchMethods._instance ??= new SearchMethods();

    /// <summary>
    /// Adds a new search parameter, strategy or way to search for a submission
    /// </summary>
    /// <param name="parameterName"> the name to find which strategy to use </param>
    /// <param name="searchQuery"> the strategy of how to find a submission </param>
    /// <param name="activationQualifier"> a function used to determine whether the search is to be performed or not. e.g. if string is not null then search </param>
    private void AddSearchParameter(SearchParam.SearchParameterTypes parameterName, Expression<Func<Submission, string, bool>> searchQuery, Func<string, bool> activationQualifier)
    {
        _searchParameters.Add(parameterName, new SearchParam { SearchQuery = searchQuery, ActivationQualifier = activationQualifier });
    }

    /// <summary>
    /// returns the search strategy or method that corresponds to the parameter name
    /// if the parameter name is not found, returns null
    /// </summary>
    /// <param name="parameterName"> name of the parameter whose strategy or method is sought</param>
    /// <returns> search strategy or method </returns>
    public SearchParam GetSearchMethodByName(SearchParam.SearchParameterTypes parameterName)
    {
        this._searchParameters.TryGetValue(parameterName, out SearchParam searchParameter);
        
        return searchParameter;
    }
    
    /// <summary>
    /// Returns whether the parameter type has been mapped to a search method
    /// </summary>
    /// <param name="parameterName"></param>
    /// <returns> if a search method for the parameter type has been added </returns>
    public bool Contains(SearchParam.SearchParameterTypes parameterName)
    {
        return this._searchParameters.ContainsKey(parameterName);
    }
    
    /// <summary>
    /// Method where all the search parameters are to be added
    /// </summary>
    private void AddAllSearchParameters()
    {
        // find by name
        AddSearchParameter(SearchParam.SearchParameterTypes.Name
            , (submission, productName) => submission.Product.Name.Contains(productName)
            , productName => !string.IsNullOrEmpty(productName));
        
        // find by model
        AddSearchParameter(SearchParam.SearchParameterTypes.Model
            , (submission, model) => submission.Product.Model.Contains(model)
            , model => !string.IsNullOrEmpty(model));
        
        // find by province
        AddSearchParameter(SearchParam.SearchParameterTypes.Province
            , (submission, province) => submission.Store.Canton.Province.Name.Contains(province)
            , province => !string.IsNullOrEmpty(province));
        
        // find by canton
        AddSearchParameter(SearchParam.SearchParameterTypes.Canton
            , (submission, canton) => submission.Store.Canton.Name.Contains(canton)
            , canton => !string.IsNullOrEmpty(canton));
        
        // find by brand
        AddSearchParameter(SearchParam.SearchParameterTypes.Brand
            , (submission, brand) => submission.Product.Brand.ToLower().Contains(brand.ToLower())
            , brand => !string.IsNullOrEmpty(brand));
    }
}