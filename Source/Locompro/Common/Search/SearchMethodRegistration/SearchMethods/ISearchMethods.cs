using System.Linq.Expressions;

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
    SearchParam GetSearchMethodByName(SearchParameterTypes parameterName);

    /// <summary>
    ///     Returns whether the parameter type has been mapped to a search method
    /// </summary>
    /// <param name="parameterName"></param>
    /// <returns> if a search method for the parameter type has been added </returns>
    bool Contains(SearchParameterTypes parameterName);
}