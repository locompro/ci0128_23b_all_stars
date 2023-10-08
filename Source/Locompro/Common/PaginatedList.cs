using Microsoft.EntityFrameworkCore;

namespace Locompro.Common
{
    /// <summary>
    /// Represents a paginated list of items of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of items in the paginated list.</typeparam>
    public class PaginatedList<T> : List<T>
    {
        /// <summary>
        /// Gets the index of the current page.
        /// </summary>
        public int PageIndex { get; private set; }

        /// <summary>
        /// Gets the total number of pages.
        /// </summary>
        public int TotalPages { get; private set; }

        /// <summary>
        /// Gets the total number of items across all pages.
        /// </summary>
        public int TotalItems { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaginatedList{T}"/> class.
        /// </summary>
        /// <param name="items">The list of items to be paginated.</param>
        /// <param name="count">The total number of items.</param>
        /// <param name="pageIndex">The index of the current page.</param>
        /// <param name="pageSize">The size of the page.</param>
        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalItems = count;

            this.AddRange(items);
        }

        /// <summary>
        /// Gets a value indicating whether there is a previous page.
        /// </summary>
        public bool HasPreviousPage => PageIndex > 1;

        /// <summary>
        /// Gets a value indicating whether there is a next page.
        /// </summary>
        public bool HasNextPage => PageIndex < TotalPages;

        /// <summary>
        /// Asynchronously creates a paginated list from a queryable source.
        /// </summary>
        /// <param name="source">The queryable source of items.</param>
        /// <param name="pageIndex">The index of the current page.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the paginated list.</returns>
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
        /// Creates a paginated list from a list source.
        /// </summary>
        /// <param name="source">The list source of items.</param>
        /// <param name="pageIndex">The index of the current page.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <returns>A <see cref="PaginatedList{T}"/> containing the paginated items.</returns>
        public static PaginatedList<T> Create(List<T> source, int pageIndex, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip(
                    (pageIndex - 1) * pageSize)
                .Take(pageSize).ToList();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}