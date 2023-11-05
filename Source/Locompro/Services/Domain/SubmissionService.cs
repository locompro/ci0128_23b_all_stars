using Locompro.Common.Search;
using Locompro.Data;
using Locompro.Models;
using Locompro.Data.Repositories;

namespace Locompro.Services.Domain;

public class SubmissionService : DomainService<Submission, SubmissionKey>, ISubmissionService
{
    private readonly ISubmissionRepository _submissionRepository;
    
    public SubmissionService(IUnitOfWork unitOfWork, ILoggerFactory loggerFactory) : base(unitOfWork, loggerFactory)
    {
        _submissionRepository = UnitOfWork.GetSpecialRepository<ISubmissionRepository>();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Submission>> GetSearchResults(SearchQueries searchQueries)
    {
        return await _submissionRepository.GetSearchResults(searchQueries);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Submission>> GetItemSubmissions(string storeName, string productName)
    {
        return await _submissionRepository.GetItemSubmissions(storeName, productName);
    }

    /// <inheritdoc />
    public async Task UpdateSubmissionRating(string userId, DateTime entryTime, int newRating)
    {
        Submission submissionToUpdate = await _submissionRepository.GetByIdAsync(new SubmissionKey() { UserId = userId, EntryTime = entryTime });
        
        PlaceNewSubmissionRating(submissionToUpdate, newRating);
        
        _submissionRepository.UpdateAsync(submissionToUpdate);
        await UnitOfWork.SaveChangesAsync();
    }
    
    /// <summary>
    /// Places the new rating in the submission
    /// Considers previous ratings and calculates a new average
    /// </summary>
    /// <param name="submissionToUpdate"> submission where rating will be placed </param>
    /// <param name="newRating"> new rating to be placed </param>
    private static void PlaceNewSubmissionRating(Submission submissionToUpdate, int newRating)
    {
        float currentRating = submissionToUpdate.Rating;
        long submissionRatingsAmount = submissionToUpdate.NumberOfRatings;
        
        submissionRatingsAmount = GetNewRatingAmount(submissionRatingsAmount, currentRating);
        currentRating = CalculateNewRating(currentRating, newRating, submissionRatingsAmount);

        submissionToUpdate.Rating = currentRating;
        submissionToUpdate.NumberOfRatings = submissionRatingsAmount;
    }
    
    /// <summary>
    /// Calculates a new rating as the average of all ratings
    /// </summary>
    /// <remarks> calculation goes as follows: ((currentAmountOfRatings*currentRating) + newRating) /
    /// newRatingsAmount </remarks>>
    /// <param name="currentRating"> submission rating before considering new rating</param>
    /// <param name="newRating"> new incoming rating to be added </param>
    /// <param name="submissionRatingsAmount"> the current amount of ratings </param>
    /// <returns> result of old rating average and new rating </returns>
    private static float CalculateNewRating(float currentRating, int newRating, long submissionRatingsAmount)
    {
        float newCalculatedRating = (submissionRatingsAmount - 1) * currentRating;
        newCalculatedRating += newRating;
        newCalculatedRating /= submissionRatingsAmount;
        
        return newCalculatedRating;
    }
    
    /// <summary>
    /// Gets the new amount of ratings
    /// </summary>
    /// <param name="currentRatingAmount"></param>
    /// <param name="currentRating"></param>
    /// <returns></returns>
    private static long GetNewRatingAmount(long currentRatingAmount, double currentRating)
    {
        if (currentRatingAmount == 0 && currentRating != 0)
        {
            currentRatingAmount++;
        }
        
        return currentRatingAmount + 1;
    }
}