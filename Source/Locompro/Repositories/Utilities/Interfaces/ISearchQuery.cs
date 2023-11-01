using System.Linq.Expressions;

namespace Locompro.SearchQueryConstruction;

public interface ISearchQuery
{
    Expression GetQueryFunction();
}