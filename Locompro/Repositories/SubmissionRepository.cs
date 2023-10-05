using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Locompro.Models;
using Locompro.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NuGet.Versioning;

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
    
    /// <summary>
    /// Get all the submissions for a product
    /// </summary>
    /// <param name="productId"></param>
    /// <returns></returns>
    public async Task<IEnumerable<Submission>> getSubmissionsForProduct (int productId)
    {
        // get all submissions for the product
        IQueryable<Submission> submissionsQuery = this.DbSet.Where(s => s.ProductId == productId);
        
        return await submissionsQuery.ToListAsync();
    }

    public async Task<Submission> getBestSubmission(int productId)
    {
        // get all submissions for the given product
        IQueryable<Submission> submissionsQuery = this.DbSet.Where(s => s.ProductId == productId);
        
        // get the logic for the best submission
        DateTime mostRecentDate = submissionsQuery.Max(s => s.EntryTime);

        // use logic to get that submission
        Submission bestSubmission = submissionsQuery.FirstOrDefault(s=>s.EntryTime == mostRecentDate);
        
        return await Task.FromResult(bestSubmission);
    }
}