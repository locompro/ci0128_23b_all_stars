namespace Locompro.Repositories.Utilities.Interfaces;

public interface IQueryBuilder
{
    public void AddSearchCriterion(ISearchCriterion searchCriterion);

    public SearchQueries GetSearchFunction();

    public void Reset();
}