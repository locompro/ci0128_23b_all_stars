using System.Reflection.PortableExecutable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Locompro.Models;
using Locompro.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Locompro.Repositories;

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
    

    public async Task<IEnumerable<Submission>> GetSubmissionsByCantonAsync(string cantonName, string provinceName)
    {
        var submissions = await DbSet
            .Include(s => s.Store)
            .ThenInclude(st => st.Canton)
            .Where(s => s.Store.Canton.Name == cantonName && s.Store.Canton.Province.Name == provinceName)
            .ToListAsync();
    
        return submissions;
    }
    
    /// <summary>
    /// Gets all submissions that contain the given product model
    /// </summary>
    /// <param name="productModel"></param>
    public async Task<IEnumerable<Submission>> GetSubmissionsByProductModelAsync(string productModel)
    {
        var submissions = await DbSet
            .Include(s => s.Product)
            .Where(s => s.Product.Model.Contains(productModel))
            .ToListAsync();
    
        return submissions;
    }

    /// <summary>
    /// Gets all submissions that contain the given product name
    /// </summary>
    /// <param name="productName"></param>
    /// <returns></returns>
    public async Task<IEnumerable<Submission>> GetSubmissionsByProductNameAsync(string productName)
    {
        IQueryable<Submission> submissionsQuery = this.DbSet
            .Include(s => s.Product)
            .Where(s => s.Product.Name.Contains(productName));
        
        return await submissionsQuery.ToListAsync();
    }
}