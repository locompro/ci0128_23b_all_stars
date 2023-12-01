using Locompro.Models.Entities;
using Locompro.Models.ViewModels;

namespace Locompro.Common.Search.SearchMethodRegistration.SearchMethods;

/// <summary>
/// Search methods for submissions
/// </summary>
public class SubmissionSearchMethods : SearchMethods<Submission, SubmissionSearchMethods>
{
    /// <inheritdoc />
    protected override void InitializeSearchMethods()
    {
        // find by name
        AddSearchParameter<string>(SearchParameterTypes.SubmissionByName
            , (submission, productName) => submission.Product.Name.Contains(productName)
            , productName => !string.IsNullOrEmpty(productName));

        // find by model
        AddSearchParameter<string>(SearchParameterTypes.SubmissionByModel
            , (submission, model) => submission.Product.Model.Contains(model)
            , model => !string.IsNullOrEmpty(model));

        // find by province
        AddSearchParameter<string>(SearchParameterTypes.SubmissionByProvince
            , (submission, province) => submission.Store.Canton.Province.Name.Contains(province)
            , province => !string.IsNullOrEmpty(province));

        // find by canton
        AddSearchParameter<string>(SearchParameterTypes.SubmissionByCanton
            , (submission, canton) => submission.Store.Canton.Name.Contains(canton)
            , canton => !string.IsNullOrEmpty(canton));

        // find by brand
        AddSearchParameter<string>(SearchParameterTypes.SubmissionByBrand
            , (submission, brand) => submission.Product.Brand.Contains(brand)
            , brand => !string.IsNullOrEmpty(brand));

        // find by category
        AddSearchParameter<string>(SearchParameterTypes.SubmissionByCategory,
            (submission, category) =>
                submission.Product.Categories.Any(existingCategory => existingCategory.Name.Contains(category)),
            category => !string.IsNullOrEmpty(category));

        // find if price is more than min price
        AddSearchParameter<long>(SearchParameterTypes.SubmissionByMinvalue
            , (submission, minVal) => submission.Price > minVal
            , minVal => minVal != 0);

        // find if price is less than max value
        AddSearchParameter<long>(SearchParameterTypes.SubmissionByMaxvalue
            , (submission, maxVal) => submission.Price < maxVal
            , maxVal => maxVal != 0);

        // find if submission has been reported an specific amount of times at minimum
        AddSearchParameter<int>(SearchParameterTypes.SubmissionByNAmountReports
            , (Submission submission, int minReportAmount) =>
                submission.UserReports != null
                && submission.UserReports.Count >= minReportAmount
                && submission.Status != SubmissionStatus.Moderated
            , minReportAmount => minReportAmount >= 0);

        // find by user id
        AddSearchParameter<string>(SearchParameterTypes.SubmissionByUserId
            , (submission, userId) => submission.UserId == userId
            , userId => !string.IsNullOrEmpty(userId));
        
        // find by distance from the user
        AddSearchFilter<MapVm>(SearchParameterTypes.SubmissionByLocationFilter
            , (submission, mapVm) =>
            {
                if (submission.Store.Location == null)
                {
                    return false;
                }
                
                return MapVm.Ratio * submission.Store.Location.Distance(mapVm.Location) <= mapVm.Distance;
            },
            mapVmParam => mapVmParam.Location != null && mapVmParam.Distance != 0);

        // find if submission has user as approver or rejecter
        AddSearchParameter<string>(SearchParameterTypes.SubmissionHasApproverOrRejecter
            , (submission, userId) =>
                submission.Approvers.All(u => u.Id != userId)
                && submission.Rejecters.All(u => u.Id != userId)
            , userId => !string.IsNullOrWhiteSpace(userId));

        // find if submission has more than max auto reports
        AddSearchParameter<int>(SearchParameterTypes.SubmissionHasMaxAutoReports
            , (submission, maxAutoReports) => submission.AutoReports.Count >= maxAutoReports
            , maxAutoReports => maxAutoReports >= 0);
    }
}