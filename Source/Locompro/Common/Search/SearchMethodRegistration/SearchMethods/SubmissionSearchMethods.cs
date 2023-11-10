using Locompro.Models.Entities;

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
        AddSearchParameter<string>(SearchParameterTypes.Name
            , (submission, productName) => submission.Product.Name.Contains(productName)
            , productName => !string.IsNullOrEmpty(productName));

        // find by model
        AddSearchParameter<string>(SearchParameterTypes.Model
            , (submission, model) => submission.Product.Model.Contains(model)
            , model => !string.IsNullOrEmpty(model));

        // find by province
        AddSearchParameter<string>(SearchParameterTypes.Province
            , (submission, province) => submission.Store.Canton.Province.Name.Contains(province)
            , province => !string.IsNullOrEmpty(province));

        // find by canton
        AddSearchParameter<string>(SearchParameterTypes.Canton
            , (submission, canton) => submission.Store.Canton.Name.Contains(canton)
            , canton => !string.IsNullOrEmpty(canton));

        // find by brand
        AddSearchParameter<string>(SearchParameterTypes.Brand
            , (submission, brand) => submission.Product.Brand.Contains(brand)
            , brand => !string.IsNullOrEmpty(brand));

        // find by category
        AddSearchParameter<string>(SearchParameterTypes.Category,
            (submission, category) =>
                submission.Product.Categories.Any(existingCategory => existingCategory.Name.Contains(category)),
            category => !string.IsNullOrEmpty(category));

        // find if price is more than min price
        AddSearchParameter<long>(SearchParameterTypes.Minvalue
            , (submission, minVal) => submission.Price > minVal
            , minVal => minVal != 0);

        // find if price is less than max value
        AddSearchParameter<long>(SearchParameterTypes.Maxvalue
            ,  (submission, maxVal) => submission.Price < maxVal
            , maxVal => maxVal != 0);
        
        // find if submission has been reported an specific amount of times at minimum
        AddSearchParameter<int>(SearchParameterTypes.HasNAmountReports
            , (Submission submission, int minReportAmount) =>
                submission.Reports != null
                && submission.Reports.Count >= minReportAmount
                && submission.Status != SubmissionStatus.Moderated
            , minReportAmount => minReportAmount >= 0);
    }
}