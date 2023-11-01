using System.Linq.Expressions;

namespace Locompro.Repositories.Utilities.Interfaces;

public interface ISearchQuery
{
    Expression GetQueryFunction();
}