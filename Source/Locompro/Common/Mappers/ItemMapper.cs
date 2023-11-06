using Locompro.Models.Dtos;
using Locompro.Models.Entities;
using Locompro.Models.ViewModels;

namespace Locompro.Common.Mappers;

public class ItemMapper : GenericMapper<SubmissionDto, List<ItemVm>>
{
    protected override List<ItemVm> BuildVm(SubmissionDto dto)
    {
        return GetItems(dto).ToList();
    }
    
    protected override SubmissionDto BuildDto(List<ItemVm> vm)
    {
        return new SubmissionDto(null, null);
    }
    
    /// <summary>
    ///     Gets all the items to be displayed in the search results
    ///     from a list of submissions
    /// </summary>
    /// <param name="submissions"></param>
    /// <returns></returns>
    private static IEnumerable<ItemVm> GetItems(SubmissionDto submissions)
    {
        var items = new List<ItemVm>();
        
        if (submissions == null || submissions.Submissions == null) return items;
        
        // Group submissions by store
        var submissionsByStore = submissions.Submissions.GroupBy(s => s.Store);

        foreach (var store in submissionsByStore)
        {
            var submissionsByProduct = store.GroupBy(s => s.Product);
            foreach (var product in submissionsByProduct) items.Add(GetItem(product, submissions.BestSubmissionQualifier));
        }

        return items;
    }

    /// <summary>
    ///     Produces an item from a group of submissions
    ///     Gets the best submission from the group of items
    ///     uses its information for the item to be shown
    /// </summary>
    /// <param name="itemGrouping"></param>
    /// <param name="bestSubmissionQualifier"></param>
    /// <returns></returns>
    private static  ItemVm GetItem(
        IGrouping<Product, Submission> itemGrouping,
        Func<IEnumerable<Submission>, Submission> bestSubmissionQualifier)
    {
        // Get best submission for its information
        var bestSubmission = bestSubmissionQualifier(itemGrouping);

        var categories = new List<string>();

        foreach (var submission in itemGrouping)
        {
            if (submission.Product.Categories == null) continue;
            categories.AddRange(submission.Product.Categories.Select(c => c.Name).ToList());
        }

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
    ///     Constructs a list of display submissions from a list of submissions
    ///     Reduces the amount of memory necesary to display submissions
    /// </summary>
    /// <param name="submissions"> submissions to be turned into display submissions</param>
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
        return DateFormatter.GetFormattedDateFromDateTime(submission.EntryTime);
    }
}
