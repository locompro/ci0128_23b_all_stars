using Locompro.Models.ViewModels;

namespace Locompro.Services;

/// <summary>
/// Defines the contract for services handling user contributions.
/// </summary>
public interface IContributionService
{
    /// <summary>
    /// Adds a new submission based on the provided view models for a store, product, submission, and associated pictures.
    /// </summary>
    /// <param name="storeVm">The view model containing the store's data.</param>
    /// <param name="productVm">The view model containing the product's data.</param>
    /// <param name="submissionVm">The view model containing the submission's data.</param>
    /// <param name="picturesVMs">A list of view models for each picture associated with the submission.</param>
    /// <returns>A task that represents the asynchronous add operation.</returns>
    Task AddSubmission(StoreVm storeVm, ProductVm productVm,
        SubmissionVm submissionVm, List<PictureVm> picturesVMs);
}