using Locompro.Data;
using Locompro.Models;
using Locompro.Models.ViewModels;
using Locompro.Services.Domain;
using Microsoft.AspNetCore.Identity;

namespace Locompro.Services;

public class ContributionService : AbstractService
{
    private const string OnlyCountry = "Costa Rica";

    private readonly CantonService _cantonService;

    private readonly StoreService _storeService;

    private readonly ProductService _productService;

    private readonly CategoryService _categoryService;

    private readonly SubmissionService _submissionService;

    public ContributionService(UnitOfWork unitOfWork, ILoggerFactory loggerFactory, CantonService cantonService,
        StoreService storeService, ProductService productService, CategoryService categoryService,
        SubmissionService submissionService) : base(unitOfWork, loggerFactory)
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
            Username = userId,
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
            return await _storeService.Get(storeViewModel.Name);
        }
        else
        {
            Canton canton = await _cantonService.Get(OnlyCountry, storeViewModel.Province, storeViewModel.Canton);

            Store store = new Store
            {
                Name = storeViewModel.Name,
                Canton = canton,
                Address = storeViewModel.Address,
                Telephone = storeViewModel.Telephone
            };

            return store;
        }
    }

    private async Task<Product> BuildProduct(ProductViewModel productViewModel)
    {
        if (productViewModel.IsNewProduct())
        {
            // TODO: Set up to add new category through select2

            Category category = await _categoryService.Get(productViewModel.Category);

            Product product = new Product()
            {
                Name = productViewModel.Name,
                Model = productViewModel.Model,
                Brand = productViewModel.Brand,
            };

            if (category != null)
            {
                product.Categories.Add(category);
            }

            return product;
        }
        else
        {
            return await _productService.Get(productViewModel.Id.GetValueOrDefault());
        }
    }
}