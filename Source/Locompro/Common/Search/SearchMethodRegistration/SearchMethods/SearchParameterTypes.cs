namespace Locompro.Common.Search.SearchMethodRegistration.SearchMethods;

/// <summary>
///     List of all possible search paramaters
///     Add one here to add a new search parameter
/// </summary>
public enum SearchParameterTypes
{
    Default,
    SubmissionByName,
    SubmissionByProvince,
    SubmissionByCanton,
    SubmissionByMinvalue,
    SubmissionByMaxvalue,
    SubmissionByCategory,
    SubmissionByModel,
    SubmissionByBrand,
    SubmissionByNAmountReports,
    SubmissionByUserId,
    SubmissionByLocationFilter,
    SubmissionHasApproverOrRejecter,
    SubmissionHasNAutoReports
}