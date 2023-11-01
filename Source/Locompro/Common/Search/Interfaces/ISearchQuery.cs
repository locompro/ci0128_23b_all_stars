using System.Linq.Expressions;

namespace Locompro.Common.Search.Interfaces;

public interface ISearchQuery
{
    Expression GetQueryFunction();
}