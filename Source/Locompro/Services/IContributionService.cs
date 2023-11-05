using Locompro.Models.ViewModels;

namespace Locompro.Services;

public interface IContributionService
{
    Task AddSubmission(StoreViewModel storeViewModel, ProductViewModel productViewModel,
        SubmissionViewModel submissionViewModel, List<PictureViewModel> picturesVMs);
}