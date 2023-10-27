using System.Reflection.PortableExecutable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Locompro.Models;
using Locompro.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="loggerFactory"></param>
        public SubmissionRepository(DbContext context, ILoggerFactory loggerFactory) :
            base(context, loggerFactory)
        {
        }
        
        /// <inheritdoc />
        public virtual async Task<IEnumerable<Submission>> GetByCantonAsync(string cantonName,
            string provinceName)
        {
            List<Submission> submissions;
            
            if (String.IsNullOrEmpty(cantonName))
            {
                submissions = await Set
                    .Include(s => s.Store)
                    .ThenInclude(st => st.Canton)
                    .Where(s => s.Store.Canton.Province.Name == provinceName)
                    .ToListAsync();
            }
            else
            {
                submissions = await Set
                    .Include(s => s.Store)
                    .ThenInclude(st => st.Canton)
                    .Where(s => s.Store.Canton.Name == cantonName && s.Store.Canton.Province.Name == provinceName)
                    .ToListAsync();
            }

            return submissions;
        }

        /// <inheritdoc />
        public virtual async Task<IEnumerable<Submission>> GetByProductModelAsync(string productModel)
        {
            var submissions = await Set
                .Include(s => s.Product)
                .Where(s => s.Product.Model.Contains(productModel))
                .ToListAsync();

            return submissions;
        }

        /// <inheritdoc />
        public virtual async Task<IEnumerable<Submission>> GetByProductNameAsync(string productName)
        {
            IQueryable<Submission> submissionsQuery = Set
                .Include(s => s.Product)
                .Where(s => s.Product.Name.Contains(productName));

            return await submissionsQuery.ToListAsync();
        }

        /// <inheritdoc />
        public virtual async Task<IEnumerable<Submission>> GetByBrandAsync(string brandName)
        {
            IQueryable<Submission> submissionsQuery = Set
                .Include(s => s.Product)
                .Where(s => s.Product.Brand.Contains(brandName));

            return await submissionsQuery.ToListAsync();
        }
    }
}