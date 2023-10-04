using System;
using Locompro.Models;
using Locompro.Data;
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
}