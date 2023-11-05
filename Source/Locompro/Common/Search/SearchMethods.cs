using System.Linq.Expressions;
using Locompro.Models;

namespace Locompro.Common.Search;

/// <summary>
/// Singleton class that is used to store all the search strategies
/// </summary>
public class SearchMethods
{
    private static SearchMethods _instance;
    private readonly Dictionary<SearchParameterTypes, SearchParam> _searchParameters;
    
    /// <summary>
    /// Singleton private constructor
    /// </summary>
    private SearchMethods()
    {
        _searchParameters = new Dictionary<SearchParameterTypes, SearchParam>();
        AddAllSearchParameters();
    }

    /// <summary>
    /// returns instance of the singleton class
    /// </summary>
    public static SearchMethods GetInstance => _instance ??= new SearchMethods();

    /// <summary>
    /// Adds a new search parameter, strategy or way to search for a submission
    /// </summary>
    /// <param name="parameterName"> the name to find which strategy to use </param>
    /// <param name="searchQuery"> the strategy of how to find a submission </param>
    /// <param name="activationQualifier"> a function used to determine whether the search is to be performed or not. e.g. if string is not null then search </param>
    private void AddSearchParameter<T>(SearchParameterTypes parameterName, Expression<Func<Submission, T, bool>> searchQuery, Func<T, bool> activationQualifier)
    {
        _searchParameters.Add(
            parameterName,
            new SearchParam
                {
                    SearchQuery = new SearchQuery<T>() { QueryFunction = searchQuery},
                    ActivationQualifier = new ActivationQualifier<T>() { QualifierFunction = activationQualifier}
                }
        );
    }

    /// <summary>
    /// returns the search strategy or method that corresponds to the parameter name
    /// if the parameter name is not found, returns null
    /// </summary>
    /// <param name="parameterName"> name of the parameter whose strategy or method is sought</param>
    /// <returns> search strategy or method </returns>
    public SearchParam GetSearchMethodByName(SearchParameterTypes parameterName)
    {
        _searchParameters.TryGetValue(parameterName, out SearchParam searchParameter);
        
        return searchParameter;
    }

    /// <summary>
    /// Returns whether the parameter type has been mapped to a search method
    /// </summary>
    /// <param name="parameterName"></param>
    /// <returns> if a search method for the parameter type has been added </returns>
    public bool Contains(SearchParameterTypes parameterName)
    {
        return _searchParameters.ContainsKey(parameterName);
    }

    /// <summary>
    /// Method where all the search parameters are to be added
    /// </summary>
    private void AddAllSearchParameters()
    {
        // find by name
        AddSearchParameter<string>(SearchParameterTypes.Name
            , (submission, productName) => submission.Product.Name.Contains(productName)
            , productName => !string.IsNullOrEmpty(productName));

        // find by model
        AddSearchParameter<string>(SearchParameterTypes.Model
            , (submission, model) => submission.Product.Model.Contains(model)
            , model => !string.IsNullOrEmpty(model));

        // find by province
        AddSearchParameter<string>(SearchParameterTypes.Province
            , (submission, province) => submission.Store.Canton.Province.Name.Contains(province)
            , province => !string.IsNullOrEmpty(province));

        // find by canton
        AddSearchParameter<string>(SearchParameterTypes.Canton
            , (submission, canton) => submission.Store.Canton.Name.Contains(canton)
            , canton => !string.IsNullOrEmpty(canton));

        // find by brand
        AddSearchParameter<string>(SearchParameterTypes.Brand
            , (submission, brand) => submission.Product.Brand.ToLower().Contains(brand.ToLower())
            , brand => !string.IsNullOrEmpty(brand));

        // find by category
        AddSearchParameter<string>(SearchParameterTypes.Category,
            ((submission, category) =>
                submission.Product.Categories.Any(existingCategory => existingCategory.Name.Equals(category))),
            category => !string.IsNullOrEmpty(category));
        
        // find if price is more than min price
        AddSearchParameter<long>(SearchParameterTypes.Minvalue
            , (submission, minVal) => submission.Price > minVal
            , minVal => minVal != 0);
       
        // find if price is less than max value
        AddSearchParameter<long>(SearchParameterTypes.Maxvalue
            , (submission, maxVal) => submission.Price < maxVal
            , maxVal => maxVal != 0);
    }
}