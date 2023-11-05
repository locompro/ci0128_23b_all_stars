using Locompro.Models.ViewModels;

namespace Locompro.Services;

public interface IContributionService
{
    Task AddSubmission(StoreVm storeVm, ProductVm productVm,
        SubmissionVm submissionVm, List<PictureVm> picturesVMs);
}