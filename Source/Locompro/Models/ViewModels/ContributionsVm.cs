using Locompro.Common.Mappers;
using Locompro.Models.Dtos;
using Locompro.Models.Entities;

namespace Locompro.Models.ViewModels;

public class ContributionsVm
{
    public ContributionsVm()
    {
    }
    /// <summary>
    ///     Constructor of ContributionViewModel based on a User object.
    /// </summary>
    /// <param name="user">The user whose information will be displayed.</param>
    public ContributionsVm(User user)
    {
        try
        {
            Profile = new ProfileVm(user);
            ItemMapper itemMapper = new();
            Contributions = itemMapper.ToVm(new SubmissionsDto(user.Submissions, GetLatestSubmission));
        }
        catch (Exception e)
        {
            throw new Exception(e + "Invalid User used to create a ContributionsVm");
        }
    }
    
    public ProfileVm Profile {get; set;}
    public List<ItemVm> Contributions { get; set; }
    
    /// <summary>
    ///     Qualifier to organize submissions when grouping into an Item
    /// </summary>
    /// <param name="submissions"></param>
    /// <returns></returns>
    private static Submission GetLatestSubmission(IEnumerable<Submission> submissions)
    {
        return submissions.MaxBy(s => s.EntryTime);
    }
}