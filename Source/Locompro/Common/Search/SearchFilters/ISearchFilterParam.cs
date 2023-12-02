using Locompro.Common.Search.SearchMethodRegistration;

namespace Locompro.Common.Search.SearchFilters;

public interface ISearchFilterParam
{
    IActivationQualifier GetActivationQualifier();

    ISearchFilterQuery GetSearchQuery();
}