using Locompro.Common.Search;
using Microsoft.EntityFrameworkCore;

namespace Locompro.Data;

public class DynamicQueryable<T> : IDynamicQueryable<T>
    where T : class
{
    private IQueryable<T> _queryable;
    
    public DynamicQueryable(IQueryable<T> queryable)
    {
        _queryable = queryable;
    }
    
    public DynamicQueryable(DbSet<T> set) {
        _queryable = set;
    }

    public async Task<IEnumerable<T>> GetResultsByAsync(ISearchQueries<T> searchQueries)
    {
        _queryable = searchQueries.ApplySearch(_queryable);
        
        return searchQueries.NoSearchFilters()?
            await _queryable.ToListAsync() :
            searchQueries.ApplySearchFilters(await _queryable.ToListAsync());
    }
}