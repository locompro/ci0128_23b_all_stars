namespace Locompro.Common.Search;

public interface ISearchQueries
{
    /// <summary>
    ///     returns if the search query is empty
    /// </summary>
    public bool IsEmpty();
    
    /// <summary>
    /// Finds the amount of query functions within
    /// </summary>
    /// <returns> amount of query functions within </returns>
    int Count();
    
    /// <summary>
    /// Applies the search queries to the queryable
    /// </summary>
    /// <remarks>
    /// After return, the result must be interpreted as a IQueryable<T>
    /// </remarks>
    /// <param name="queryable"> queryable to which the search queries will be applied </param>
    /// <returns> queryable with search queries applied </returns>
    IQueryable ApplySearch(IQueryable queryable);
}