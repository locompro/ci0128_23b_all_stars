using Locompro.Data;
using Locompro.Models.Entities;
using Locompro.Models.ViewModels;
using Locompro.Services.Domain;
using NetTopologySuite.Geometries;

namespace Locompro.Services;

/// <summary>
/// Provides services for handling user contributions, such as adding new submissions.
/// </summary>
public class ContributionService : Service, IContributionService
{
    private const string OnlyCountry = "Costa Rica";

    private readonly ICantonService _cantonService;

    private readonly INamedEntityDomainService<Category, string> _categoryService;

    private readonly INamedEntityDomainService<Product, int> _productService;

    private readonly INamedEntityDomainService<Store, string> _storeService;

    private readonly ISubmissionService _submissionService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ContributionService"/> class.
    /// </summary>
    /// <param name="loggerFactory">The factory used for creating loggers.</param>
    /// <param name="cantonService">The service for canton-related operations.</param>
    /// <param name="storeService">The service for store-related operations.</param>
    /// <param name="productService">The service for product-related operations.</param>
    /// <param name="categoryService">The service for category-related operations.</param>
    /// <param name="submissionService">The service for submission-related operations.</param>
    public ContributionService(ILoggerFactory loggerFactory, ICantonService cantonService,
        INamedEntityDomainService<Store, string> storeService, INamedEntityDomainService<Product, int> productService,
        INamedEntityDomainService<Category, string> categoryService, ISubmissionService submissionService)
        : base(loggerFactory)
    {
        _cantonService = cantonService;
        _storeService = storeService;
        _productService = productService;
        _categoryService = categoryService;
        _submissionService = submissionService;
    }

    /// <inheritdoc />
    /// <summary>
    /// Adds a new submission to the system based on the provided view models.
    /// </summary>
    /// <param name="storeVm">The view model containing the store data.</param>
    /// <param name="productVm">The view model containing the product data.</param>
    /// <param name="submissionVm">The view model containing the submission data.</param>
    /// <param name="picturesVMs">The list of view models for the pictures associated with the submission.</param>
    /// <returns>A task representing the asynchronous operation of adding a submission.</returns>
    public async Task AddSubmission(StoreVm storeVm, ProductVm productVm,
        SubmissionVm submissionVm, List<PictureVm> picturesVMs)
    {
        var store = await BuildStore(storeVm);

        var product = await BuildProduct(productVm);

        var entryTime = DateTime.Now;

        var pictures = BuildPictures(picturesVMs, entryTime, submissionVm.UserId);

        var submission = new Submission
        {
            UserId = submissionVm.UserId,
            EntryTime = entryTime,
            Price = submissionVm.Price,
            Description = submissionVm.Description,
            Store = store,
            Product = product,
            Pictures = pictures
        };

        await _submissionService.Add(submission);
    }

    private async Task<Store> BuildStore(StoreVm storeVm)
    {
        if (storeVm.IsExistingStore()) return await _storeService.Get(storeVm.SName);

        var canton = await _cantonService.Get(OnlyCountry, storeVm.Province, storeVm.Canton);

        var store = new Store
        {
            Name = storeVm.SName,
            Canton = canton,
            Address = storeVm.Address,
            Telephone = storeVm.Telephone,
            /*Location = new Point(storeVm.Latitude, storeVm.Longitude)
            {
                SRID = 4326
            },*/
        };

        return store;
    }

    private async Task<Product> BuildProduct(ProductVm productVm)
    {
        if (productVm.IsExistingProduct()) return await _productService.Get(productVm.Id);

        var category = await _categoryService.Get(productVm.Category);

        var product = new Product
        {
            Name = productVm.PName,
            Model = productVm.Model,
            Brand = productVm.Brand,
            Categories = new List<Category>()
        };

        if (category != null) product.Categories.Add(category);

        return product;
    }

    private static List<Picture> BuildPictures(List<PictureVm> pictureVms, DateTime entryTime, string userId)
    {
        var pictures = new List<Picture>();
        var pictureIndex = 0;

        foreach (var pictureVm in pictureVms)
        {
            pictures.Add(
                new Picture
                {
                    Index = pictureIndex,
                    SubmissionUserId = userId,
                    SubmissionEntryTime = entryTime,
                    PictureTitle = pictureVm.Name,
                    PictureData = pictureVm.PictureData
                });

            pictureIndex++;
        }

        return pictures;
    }
}