using System.Linq.Expressions;
using Locompro.Models;

namespace Locompro.Repositories.Utilities;

/// <summary>
/// Class for encapsulating all data related to a search criterion
/// </summary>
public class SearchQuery
{
    public List<Expression<Func<Submission, bool>>> SearchQueryFunctions { get; init; }
}