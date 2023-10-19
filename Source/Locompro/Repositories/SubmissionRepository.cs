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
using Microsoft.Extensions.Logging;

namespace Locompro.Repositories
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
    public class SubmissionRepository : AbstractRepository<Submission, SubmissionKey>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="loggerFactory"></param>
        public SubmissionRepository(LocomproContext context, ILoggerFactory loggerFactory) :
            base(context, loggerFactory)
        {
        }
        
        public virtual async Task<IEnumerable<Submission>> GetSearchResults(Func<Submission, bool> searchQuery)
        {
            
            IEnumerable<Submission> submissionsResults = this.DbSet
                .Include(submission => submission.Product)
                .Where(searchQuery);

            return await Task.FromResult(submissionsResults.ToList());
        }
    }
}
