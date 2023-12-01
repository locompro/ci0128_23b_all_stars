using System.Linq.Expressions;
using Locompro.Common.Search.SearchFilters;

namespace Locompro.Common.Search.SearchMethodRegistration.SearchMethods;

/// <summary>
/// Inteface for all SearchMethods
/// </summary>
public interface ISearchMethods
{
    /// <summary>
    ///     returns the search strategy or method that corresponds to the parameter name
    ///     if the parameter name is not found, returns null
    /// </summary>
    /// <param name="parameterName"> name of the parameter whose strategy or method is sought</param>
    /// <returns> search strategy or method </returns>
    ISearchParam GetSearchMethodByName(SearchParameterTypes parameterName);

    /// <summary>
    ///     Returns whether the parameter type has been mapped to a search method
    /// </summary>
    /// <param name="parameterName"></param>
    /// <returns> if a search method for the parameter type has been added </returns>
    bool Contains(SearchParameterTypes parameterName);

    /// <summary>
    ///     Returns the search filter that corresponds to the parameter name
    /// </summary>
    /// <param name="parameterName"></param>
    /// <returns></returns>
    ISearchFilterParam GetSearchFilterByName(SearchParameterTypes parameterName);

    /// <summary>
    ///     Returns whether the parameter type has been mapped to a search filter
    /// </summary>
    /// <param name="parameterName"></param>
    /// <returns></returns>
    bool ContainsSearchFilter(SearchParameterTypes parameterName);
}