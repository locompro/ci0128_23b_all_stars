namespace Locompro.Common.Search.SearchFilters;

public interface ISearchFilterQuery
{
    Func<dynamic, dynamic, bool> GetQueryFunction();
}