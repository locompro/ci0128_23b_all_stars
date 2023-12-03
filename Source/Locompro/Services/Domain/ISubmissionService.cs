using Locompro.Common.Search;
using Locompro.Data.Repositories;
using Locompro.Models;
using Locompro.Models.Dtos;
using Locompro.Models.Entities;
using Locompro.Models.ViewModels;

namespace Locompro.Services.Domain;

public interface ISubmissionService : IDomainService<Submission, SubmissionKey>
{
    /// <summary>
    ///  Gets all submissions for a given user ID
    /// </summary>
    /// <param name="userId">User Id for which to retrieve all submissions</param>
    /// <returns>All submissions for a user</returns>
    Task<IEnumerable<Submission>> GetByUserId(string userId);
    
    /// <summary>
    ///     Gets the search results submissions according to the list of search criteria or queries to be used
    /// </summary>
    /// <param name="searchQueries"> search queries, criteria or strategies to be used to find the desired submissions</param>
    /// <returns></returns>
    Task<IEnumerable<Submission>> GetSearchResults(ISearchQueries<Submission> searchQueries);

    /// <summary>
    ///     Gets the submissions for a given Item
    /// </summary>
    /// <param name="storeName"></param>
    /// <param name="productName"></param>
    /// <returns></returns>
    Task<IEnumerable<Submission>> GetItemSubmissions(string storeName, string productName);

    /// <summary>
    ///     Updates the rating of a submission
    /// </summary>
    /// <param name="ratingVm"> view model with submission to replace info and new rating </param>
    /// <returns></returns>
    Task UpdateSubmissionRating(RatingVm ratingVm);

    /// <summary>
    /// Deletes submission
    /// </summary>
    /// <param name="submissionKey"> key of submission to be deleted</param>
    /// <returns></returns>
    Task DeleteSubmissionAsync(SubmissionKey submissionKey);

    /// <summary>
    /// Updated the status of a submission
    /// </summary>
    /// <param name="submissionKey"> key of submission to be updated</param>
    /// <param name="submissionStatus"> new status of submission </param>
    /// <returns></returns>
    Task UpdateSubmissionStatusAsync(SubmissionKey submissionKey, SubmissionStatus submissionStatus);

    /// <summary>
    /// Adds a user to list of submission approvers by their user ID.
    /// </summary>
    /// <param name="submissionKey">key for submission to add approver to.</param>
    /// <param name="userId">ID of user to add as approver.</param>
    Task AddSubmissionApprover(SubmissionKey submissionKey, string userId);

    /// <summary>
    /// Adds a user to list of submission rejecters by their user ID.
    /// </summary>
    /// <param name="submissionKey">key for submission to add rejecter to.</param>
    /// <param name="userId">ID of user to add as rejecter.</param>
    Task AddSubmissionRejecter(SubmissionKey submissionKey, string userId);

    Task<List<ShoppingListProductDto>> GetProductSummary(List<ProductDto> products);

    Task<List<SummaryStoreDto>> GetStoreSummary(List<ProductDto> products);
}