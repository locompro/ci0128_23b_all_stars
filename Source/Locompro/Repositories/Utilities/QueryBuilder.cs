
namespace Locompro.Repositories.Utilities;
using System.Collections.Generic;
using Locompro.Models;

public class QueryBuilder
{
    private readonly Dictionary<SearchParam.SearchParameterTypes, SearchParam> _searchParameters;
    private readonly List<Func<Submission, bool>> _searchCriteriaFunctions;
    private readonly List<(SearchParam.SearchParameterTypes, string)> _searchCriteria;
    
    public QueryBuilder()
    {
        this._searchCriteria = new List<(SearchParam.SearchParameterTypes, string)>();
        _searchCriteriaFunctions = new List<Func<Submission, bool>>();
        _searchParameters = new Dictionary<SearchParam.SearchParameterTypes, SearchParam>();
        this.AddAllSearchParameters();
    }

    private void AddSearchParameter(SearchParam.SearchParameterTypes parameterName, Func<Submission, string, bool> searchQuery, Func<string, bool> activationQualifier)
    {
        _searchParameters.Add(parameterName, new SearchParam { SearchQuery = searchQuery, ActivationQualifier = activationQualifier });
    }
    
    public void AddSearchCriterion(SearchParam.SearchParameterTypes parameterName, string parameterValue)
    {
        this._searchCriteria.Add((parameterName, parameterValue));
    }
    private void Compose()
    {
        // for each of the criterion in the unfiltered list
        foreach ((SearchParam.SearchParameterTypes, string) searchCriterion in _searchCriteria)
        {
            // if not within the internal map
            if (!this._searchParameters.ContainsKey(searchCriterion.Item1))
            {
                // might be better to throw an exception here, for now just ignore
                continue;
            }
            
            // get the search parameter that corresponds to the criterion
            SearchParam searchParameter = this._searchParameters[searchCriterion.Item1];
            
            // if according to its activation qualifier
            if (searchParameter.ActivationQualifier(searchCriterion.Item2))
            {
                // add its search query to the filtered list
                _searchCriteriaFunctions.Add(submission => searchParameter.SearchQuery(submission, searchCriterion.Item2));
            }
        }
    }

    private void AddAllSearchParameters()
    {
        AddSearchParameter(SearchParam.SearchParameterTypes.Name
            , (submission, productName) => submission.Product.Name.ToLower().Contains(productName.ToLower())
            , productName => !string.IsNullOrEmpty(productName));
        
        AddSearchParameter(SearchParam.SearchParameterTypes.Model
            , (submission, model) => submission.Product.Model.ToLower().Contains(model.ToLower())
            , model => !string.IsNullOrEmpty(model));
        
        AddSearchParameter(SearchParam.SearchParameterTypes.Province
            , (submission, province) => submission.Store.Canton.Province.Name.ToLower().Contains(province.ToLower())
            , province => !string.IsNullOrEmpty(province));
        
        AddSearchParameter(SearchParam.SearchParameterTypes.Canton
            , (submission, canton) => submission.Store.Canton.Name.ToLower().Contains(canton.ToLower())
            , canton => !string.IsNullOrEmpty(canton));
        
        AddSearchParameter(SearchParam.SearchParameterTypes.Brand
            , (submission, brand) => submission.Product.Brand.ToLower().Contains(brand.ToLower())
            , brand => !string.IsNullOrEmpty(brand));
    }
    public Func<Submission, bool> GetSearchFunction()
    {
        Compose();
        Func<Submission, bool> searchQuery =
            _searchCriteriaFunctions.Aggregate(
                (current, next) => (submission => current(submission) && next(submission)));
        this.Reset();
        return searchQuery;
    }
    
    public void Reset()
    {
        this._searchCriteria.Clear();
        _searchCriteriaFunctions.Clear();
    }
    
}