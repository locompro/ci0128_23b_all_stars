using System.Linq.Expressions;
using Locompro.Models;

namespace Locompro.Common.Search;

/// <summary>
/// Class for encapsulating all data related to a search criterion
/// If there were other types of criteria or functions to be used, then add to this class
/// </summary>
public class SearchQueries
{
    public List<Expression<Func<Submission, bool>>> SearchQueryFunctions { get; init; }
    
    public bool IsEmpty => this.SearchQueryFunctions.Count == 0;
}