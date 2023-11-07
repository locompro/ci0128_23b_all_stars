namespace Locompro.Common.Search.QueryBuilder;

public interface IQueryBuilder
{
    public void AddSearchCriterion(ISearchCriterion searchCriterion);

    public SearchQueries GetSearchFunction();

    public void Reset();
}