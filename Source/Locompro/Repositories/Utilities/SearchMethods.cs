using System.Linq.Expressions;
using Locompro.Models;

namespace Locompro.Repositories.Utilities;

/// <summary>
/// Singleton class that is used to store all the search strategies
/// </summary>
public class SearchMethods
{
    private static SearchMethods _instance;
    public Dictionary<SearchParam.SearchParameterTypes, SearchParam> SearchParameters { get; }
    
    /// <summary>
    /// Singleton private constructor
    /// </summary>
    private SearchMethods()
    {
        this.SearchParameters = new Dictionary<SearchParam.SearchParameterTypes, SearchParam>();
        this.AddAllSearchParameters();
    }
    
    /// <summary>
    /// returns instance of the singleton class
    /// </summary>
    public static SearchMethods GetInstance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new SearchMethods();
            }
            return _instance;
        }
    }
    
    /// <summary>
    /// Adds a new search parameter, strategy or way to search for a submission
    /// </summary>
    /// <param name="parameterName"> the name to find which strategy to use </param>
    /// <param name="searchQuery"> the strategy of how to find a submission </param>
    /// <param name="activationQualifier"> a function used to determine whether the search is to be performed or not. e.g. if string is not null then search </param>
    private void AddSearchParameter(SearchParam.SearchParameterTypes parameterName, Expression<Func<Submission, string, bool>> searchQuery, Func<string, bool> activationQualifier)
    {
        SearchParameters.Add(parameterName, new SearchParam { SearchQuery = searchQuery, ActivationQualifier = activationQualifier });
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