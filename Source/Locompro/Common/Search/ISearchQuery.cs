using System.Linq.Expressions;

namespace Locompro.Common.Search;

public interface ISearchQuery
{
    Expression GetQueryFunction();
}