using Locompro.Data;
using Locompro.Models;
using Locompro.Models.ViewModels;
using Locompro.Services.Domain;

namespace Locompro.Services;

public class ContributionService : AbstractService
{
    private const string OnlyCountry = "Costa Rica";

    private readonly CantonService _cantonService;

    private readonly StoreService _storeService;

    private readonly ProductService _productService;

    private readonly CategoryService _categoryService;

    private readonly ISubmissionService _submissionService;

    public ContributionService(IUnitOfWork unitOfWork, ILoggerFactory loggerFactory, CantonService cantonService,
        StoreService storeService, ProductService productService, CategoryService categoryService,
        ISubmissionService submissionService) : base(unitOfWork, loggerFactory)
    {
        _cantonService = cantonService;
        _storeService = storeService;
        _productService = productService;
        _categoryService = categoryService;
        _submissionService = submissionService;
    }

    public async Task AddSubmission(StoreViewModel storeViewModel, ProductViewModel productViewModel,
        string description, int price, string userId)
    {
        Store store = await BuildStore(storeViewModel);

        Product product = await BuildProduct(productViewModel);

        Submission submission = new Submission()
        {
            UserId = userId,
            EntryTime = DateTime.Now,
            Price = price,
            Description = description,
            Store = store,
            Product = product
        };

        await _submissionService.Add(submission);
    }

    private async Task<Store> BuildStore(StoreViewModel storeViewModel)
    {
        if (storeViewModel.IsExistingStore())
        {
            return await _storeService.Get(storeViewModel.SName);
        }

        Canton canton = await _cantonService.Get(OnlyCountry, storeViewModel.Province, storeViewModel.Canton);

        Store store = new Store
        {
            Name = storeViewModel.SName,
            Canton = canton,
            Address = storeViewModel.Address,
            Telephone = storeViewModel.Telephone
        };

        return store;
    }

    private async Task<Product> BuildProduct(ProductViewModel productViewModel)
    {
        if (productViewModel.IsExistingProduct())
        {
            return await _productService.Get(productViewModel.Id);
        }
        
        Category category = await _categoryService.Get(productViewModel.Category);

        Product product = new Product()
        {
            Name = productViewModel.PName,
            Model = productViewModel.Model,
            Brand = productViewModel.Brand,
            Categories = new List<Category>()
        };

        if (category != null)
        {
            product.Categories.Add(category);
        }

        return product;
    }
}