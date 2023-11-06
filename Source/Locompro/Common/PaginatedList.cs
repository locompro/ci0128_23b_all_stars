using Microsoft.EntityFrameworkCore;

namespace Locompro.Common;

/// <summary>
///     A paginated list of items.
/// </summary>
/// <typeparam name="T"></typeparam>
public class PaginatedList<T> : List<T>
{
    public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);

        AddRange(items);
    }

    public int PageIndex { get; }
    public int TotalPages { get; }

    public int TotalItems { get; }

    public bool HasPreviousPage => PageIndex > 1;

    public bool HasNextPage => PageIndex < TotalPages;

    /// <summary>
    ///     Creates a paginated list from a queryable source
    /// </summary>
    /// <param name="source"></param>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    public static async Task<PaginatedList<T>> CreateAsync(
        IQueryable<T> source, int pageIndex, int pageSize)
    {
        var count = await source.CountAsync();
        var items = await source.Skip(
                (pageIndex - 1) * pageSize)
            .Take(pageSize).ToListAsync();
        return new PaginatedList<T>(items, count, pageIndex, pageSize);
    }

    /// <summary>
    ///     Creates a paginated list from a list source
    /// </summary>
    /// <param name="source"></param>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    public static PaginatedList<T> Create(List<T> source, int pageIndex, int pageSize)
    {
        var count = source.Count();
        var items = source.Skip(
                (pageIndex - 1) * pageSize)
            .Take(pageSize).ToList();
        return new PaginatedList<T>(items, count, pageIndex, pageSize);
    }
}