using System.Reflection.PortableExecutable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Locompro.Models;
using Locompro.Data;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using Locompro.Repositories.Utilities;

namespace Locompro.Data.Repositories
{
    public struct SubmissionKey
    {
        public string CountryName { get; set; }
        public DateTime EntryTime { get; set; }
    }

    /// <summary>
    /// Repository for Submission entities.
    /// Key is a tuple of the country name and the Datetime of the submission.
    /// </summary>
    public class SubmissionRepository : CrudRepository<Submission, SubmissionKey>, ISubmissionRepository
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="loggerFactory"></param>
        public SubmissionRepository(DbContext context, ILoggerFactory loggerFactory) :
            base(context, loggerFactory)
        {
        }
        
        /// <summary>
        /// Gets the search results submissions according to the list of search criteria or queries to be used
        /// </summary>
        /// <param name="searchQueries"> search queries, criteria or strategies to be used to find the desired submissions</param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<Submission>> GetSearchResults(SearchQuery searchQueries)
        {
            // initiate the query
            IQueryable<Submission> submissionsResults = this.DbSet
                .Include(submission => submission.Product);
            
            // append the search queries to the query
            submissionsResults = searchQueries.SearchQueryFunctions.Aggregate(submissionsResults, (current, query) => current.Where(query));

            // get and return the results
            return await submissionsResults.ToListAsync();
        }
    }
}
