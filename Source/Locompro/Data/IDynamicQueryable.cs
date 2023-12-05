using Locompro.Common.Search;

namespace Locompro.Data;

public interface IDynamicQueryable<T>
{
    Task<IEnumerable<T>> GetResultsByAsync(ISearchQueries<T> searchQueries);
}