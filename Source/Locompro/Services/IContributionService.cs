using Locompro.Models.ViewModels;

namespace Locompro.Services;

public interface IContributionService
{
    Task AddSubmission(StoreViewModel storeViewModel, ProductViewModel productViewModel,
        string description, int price, string userId);
}