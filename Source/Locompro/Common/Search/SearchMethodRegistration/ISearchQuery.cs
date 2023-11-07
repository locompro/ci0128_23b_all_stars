using System.Linq.Expressions;

namespace Locompro.Common.Search.SearchMethodRegistration;

public interface ISearchQuery
{
    /// <summary>
    ///     Gets the internal search function declared by the generic class
    /// </summary>
    /// <returns></returns>
    Expression GetQueryFunction();
}