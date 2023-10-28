using System.Linq.Expressions;
using Locompro.Models;
namespace Locompro.Repositories.Utilities;

/// <summary>
/// A search parameter or way to search for a submission
/// To add a new search parameter, add a new enum to the SearchParameterTypes enum and add a new entry to the SearchParameters dictionary
/// </summary>
public class SearchParam
{
    /// <summary>
    /// List of all possible search paramaters
    /// Add one here to add a new search parameter
    /// </summary>
    public enum SearchParameterTypes
    {
        Default,
        Name,
        Province,
        Canton,
        Minvalue,
        Maxvalue,
        Category,
        Model,
        Brand
    }
    
    /// <summary>
    /// Function or expression of how to find a submission
    /// </summary>
    public Expression<Func<Submission, string, bool>> SearchQuery { get; set; }
    
    /// <summary>
    /// Function or expression of whether to perform the search or not
    /// </summary>
    public Func<string, bool> ActivationQualifier { get; set; }
}