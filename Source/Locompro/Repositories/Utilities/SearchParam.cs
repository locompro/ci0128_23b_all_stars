using System.Linq.Expressions;
using Locompro.Models;
namespace Locompro.Repositories.Utilities;

public class SearchParam
{
    public enum SearchParameterTypes
    {
        Name,
        Province,
        Canton,
        Minvalue,
        Maxvalue,
        Category,
        Model,
        Brand
    }
    public Func<Submission, string, bool> SearchQuery { get; set; }
    public Func<string, bool> ActivationQualifier { get; set; }
}