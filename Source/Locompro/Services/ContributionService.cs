using Locompro.Data;
using Locompro.Models;
using Locompro.Models.Entities;
using Locompro.Models.ViewModels;
using Locompro.Pages.Util;
using Locompro.Services.Domain;

namespace Locompro.Services;

public class ContributionService : Service, IContributionService
{
    private const string OnlyCountry = "Costa Rica";

    private readonly ICantonService _cantonService;

    private readonly INamedEntityDomainService<Store, string> _storeService;

    private readonly INamedEntityDomainService<Product, int> _productService;

    private readonly INamedEntityDomainService<Category, string> _categoryService;

    private readonly ISubmissionService _submissionService;

    public ContributionService(IUnitOfWork unitOfWork, ILoggerFactory loggerFactory, ICantonService cantonService,
        INamedEntityDomainService<Store, string> storeService, INamedEntityDomainService<Product, int> productService,
        INamedEntityDomainService<Category, string> categoryService, ISubmissionService submissionService)
        : base(unitOfWork, loggerFactory)
    {
        _cantonService = cantonService;
        _storeService = storeService;
        _productService = productService;
        _categoryService = categoryService;
        _submissionService = submissionService;
    }

    public async Task AddSubmission(StoreVm storeVm, ProductVm productVm,
        SubmissionVm submissionVm, List<PictureVm> picturesVMs)
    {
        Store store = await BuildStore(storeVm);

        Product product = await BuildProduct(productVm);

        DateTime entryTime = DateTime.Now;

        List<Picture> pictures = BuildPictures(picturesVMs, entryTime, submissionVm.UserId);
        
        Submission submission = new Submission()
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
        if (storeVm.IsExistingStore())
        {
            return await _storeService.Get(storeVm.SName);
        }

        Canton canton = await _cantonService.Get(OnlyCountry, storeVm.Province, storeVm.Canton);

        Store store = new Store
        {
            Name = storeVm.SName,
            Canton = canton,
            Address = storeVm.Address,
            Telephone = storeVm.Telephone
        };

        return store;
    }

    private async Task<Product> BuildProduct(ProductVm productVm)
    {
        if (productVm.IsExistingProduct())
        {
            return await _productService.Get(productVm.Id);
        }
        
        Category category = await _categoryService.Get(productVm.Category);

        Product product = new Product()
        {
            Name = productVm.PName,
            Model = productVm.Model,
            Brand = productVm.Brand,
            Categories = new List<Category>()
        };

        if (category != null)
        {
            product.Categories.Add(category);
        }

        return product;
    }

    private static List<Picture> BuildPictures(List<PictureVm> pictureVms, DateTime entryTime, string userId)
    {
        List<Picture> pictures = new List<Picture>();
        int pictureIndex = 0;
        
        foreach (PictureVm pictureVm in pictureVms) 
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