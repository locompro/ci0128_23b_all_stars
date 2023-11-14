using Locompro.Models.Entities;
using Microsoft.Build.Execution;

namespace Locompro.Common.Search.SearchMethodRegistration.SearchMethods;

public class SearchMethodsFactory
{
    private static SearchMethodsFactory _instance;

    private readonly Dictionary<Type, ISearchMethods> _typedSearchMethods;
    
    public static SearchMethodsFactory GetInstance()
    {
        return _instance ??= new SearchMethodsFactory();
    }

    protected SearchMethodsFactory()
    {
        _typedSearchMethods = new Dictionary<Type, ISearchMethods>();
        RegisterAllTypedSearchMethods();
    }

    public ISearchMethods Create<T>()
    {
        Type type = typeof(T);
        _typedSearchMethods.TryGetValue(type, out var searchMethods);
        return searchMethods;
    }

    private void RegisterAllTypedSearchMethods()
    {
        _typedSearchMethods.Add(typeof(Submission), SubmissionSearchMethods.GetInstance());
    }
}