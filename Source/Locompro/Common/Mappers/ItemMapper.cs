using System.Globalization;
using Locompro.Models.Dtos;
using Locompro.Models.Entities;
using Locompro.Models.ViewModels;

namespace Locompro.Common.Mappers;

/// <summary>
/// Maps a Submission DTO to a list of ItemViewModels
/// </summary>
public class ItemMapper : GenericMapper<SubmissionsDto, List<ItemVm>>
{
    protected override List<ItemVm> BuildVm(SubmissionsDto dto)
    {
        if (dto?.Submissions == null || !dto.Submissions.Any() || dto.BestSubmissionQualifier == null)
        {
            return new List<ItemVm>();
        }

        return GetItems(dto).ToList();
    }

    protected override SubmissionsDto BuildDto(List<ItemVm> vm) => new(null, null);

    /// <summary>
    ///     Gets all the items to be displayed in the search results
    ///     from a list of submissionses
    /// </summary>
    /// <param name="submissionses"></param>
    /// <returns></returns>
    private static IEnumerable<ItemVm> GetItems(SubmissionsDto submissionses)
    {
        var items = new List<ItemVm>();

        if (submissionses?.Submissions == null) return items;

        // Group submissionses by store
        var submissionsByStore = submissionses.Submissions.GroupBy(s => s.Store);

        items.AddRange(from store in submissionsByStore
            from product in store.GroupBy(s => s.Product)
            select GetItem(product, submissionses.BestSubmissionQualifier));

        return items;
    }

    /// <summary>
    ///     Produces an item from a group of submissionses
    ///     Gets the best submission from the group of items
    ///     uses its information for the item to be shown
    /// </summary>
    /// <param name="itemGrouping"></param>
    /// <param name="bestSubmissionQualifier"></param>
    /// <returns></returns>
    private static ItemVm GetItem(
        IGrouping<Product, Submission> itemGrouping,
        Func<IEnumerable<Submission>, Submission> bestSubmissionQualifier)
    {
        // Get best submission for its information
        var bestSubmission = bestSubmissionQualifier(itemGrouping);

        var categories = new List<string>();
        Dictionary<string, string> categoriesMap = null;

        foreach (var submission in itemGrouping)
        {
            categoriesMap = submission.Product.Categories.ToDictionary(c => c.Name, c => c.Name);
        }

        if (categoriesMap != null) categories.AddRange(categoriesMap.Values);

        var item = new ItemVm(
            bestSubmission,
            GetFormattedDate
        )
        {
            Submissions = GetDisplaySubmissions(itemGrouping.ToList()),
            Categories = categories
        };

        return item;
    }

    /// <summary>
    ///     Constructs a list of display submissionses from a list of submissionses
    ///     Reduces the amount of memory necesary to display submissionses
    /// </summary>
    /// <param name="submissions"> submissionses to be turned into display submissionses</param>
    /// <returns></returns>
    private static List<SubmissionVm> GetDisplaySubmissions(List<Submission> submissions)
    {
        var displaySubmissions = new List<SubmissionVm>();

        foreach (var submission in submissions) displaySubmissions.Add(new SubmissionVm(submission, GetFormattedDate));

        return displaySubmissions;
    }

    /// <summary>
    ///     Extracts from entry time, the date in the format yyyy-mm-dd
    ///     to be shown in the results page
    /// </summary>
    /// <param name="submission"></param>
    /// <returns></returns>
    private static string GetFormattedDate(Submission submission)
    {
        return submission.EntryTime.ToString("yyyy-MM-dd HH:mm:ss.fffffff", CultureInfo.InvariantCulture);
    }
}