using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Locompro.Models.Results;
/// <summary>
///   a result of the MostReportedUsers table value function
/// </summary>
[Keyless]
public class MostReportedUsersResult
{
    /// <summary>
    ///  user name 
    /// </summary>
    public string UserName { get; set; }
    /// <summary>
    ///  how many times times the user has been reported
    /// </summary>
    public int ReportedSubmissionCount { get; set; }
    /// <summary>
    ///  how many submissions the user has made
    /// </summary>
    public int TotalUserSubmissions { get; set; }
    
    public Single UserRating { get; set; }
    
}