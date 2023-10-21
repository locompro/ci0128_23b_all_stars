using System.Linq.Expressions;
using Locompro.Models;

namespace Locompro.Repositories.Utilities;

public class SearchQuery
{
    public List<Expression<Func<Submission, bool>>> searchQueryFunctions { get; init; }
}