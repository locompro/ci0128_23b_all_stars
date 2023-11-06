using Locompro.Data;
using Locompro.Data.Repositories;
using Locompro.Models.Entities;
using Locompro.Services;
using Locompro.Services.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Locompro.Tests.Services;

[TestFixture]
public class AdvancedSearchTest
{
    [SetUp]
    public void SetUp()
    {
        _loggerFactory = LoggerFactory.Create(builder => { });

        var options = new DbContextOptionsBuilder<LocomproContext>()
            .UseInMemoryDatabase("InMemoryDbForTesting")
            .Options;

        _context = new LocomproContext(options);
        _context.Database.EnsureDeleted(); // Make sure the db is clean
        _context.Database.EnsureCreated();

        var costaRica = new Country { Name = "Costa Rica" };

        _context.Set<Country>().Add(costaRica);

        var sanJose = new Province { CountryName = "Costa Rica", Name = "San José", Country = costaRica };
        var alajuela = new Province { CountryName = "Costa Rica", Name = "Alajuela", Country = costaRica };

        _context.Set<Province>().Add(sanJose);
        _context.Set<Province>().Add(alajuela);

        var sanJoseCanton = new Canton { ProvinceName = "San José", Name = "San José", Province = sanJose };
        var tibasCanton = new Canton { ProvinceName = "San José", Name = "Tibás", Province = sanJose };
        var desamparadosCanton = new Canton { ProvinceName = "San José", Name = "Desamparados", Province = sanJose };
        var alajuelaCanton = new Canton { ProvinceName = "Alajuela", Name = "Alajuela", Province = alajuela };
        var sanRamonCanton = new Canton { ProvinceName = "Alajuela", Name = "San Ramón", Province = alajuela };

        _context.Set<Canton>().Add(sanJoseCanton);
        _context.Set<Canton>().Add(tibasCanton);
        _context.Set<Canton>().Add(desamparadosCanton);
        _context.Set<Canton>().Add(alajuelaCanton);
        _context.Set<Canton>().Add(sanRamonCanton);

        new CrudRepository<User, string>(_context, _loggerFactory);

        ICrudRepository<Country, string> countryRepostory =
            new CrudRepository<Country, string>(_context, _loggerFactory);
        ICrudRepository<Category, string> categoryRepository =
            new CrudRepository<Category, string>(_context, _loggerFactory);

        var unitOfWork = new UnitOfWork(_loggerFactory, _context);
        unitOfWork.RegisterRepository(countryRepostory);
        unitOfWork.RegisterRepository(categoryRepository);

        _categoryService = new NamedEntityDomainService<Category, string>(unitOfWork, _loggerFactory);
        _countryService = new NamedEntityDomainService<Country, string>(unitOfWork, _loggerFactory);

        _advancedSearchService = new AdvancedSearchInputService(_countryService, _categoryService);

        var category1 = new Category { Name = "Sombreros" };
        var category2 = new Category { Name = "Zapatos" };
        var category3 = new Category { Name = "Ropa" };
        var category4 = new Category { Name = "Accesorios" };

        _context.Set<Category>().Add(category1);
        _context.Set<Category>().Add(category2);
        _context.Set<Category>().Add(category3);
        _context.Set<Category>().Add(category4);

        _context.SaveChanges();
    }

    private ILoggerFactory _loggerFactory;
    private LocomproContext _context;
    private INamedEntityDomainService<Country, string> _countryService;
    private INamedEntityDomainService<Category, string> _categoryService;
    private AdvancedSearchInputService _advancedSearchService;

    /// <summary>
    ///     Tests that all provinces in the database are provided
    ///     <author>Joseph Stuart Valverde Kong C18100</author>
    /// </summary>
    [Test]
    public async Task GetProvinces_ProvidesProvinces()
    {
        await _advancedSearchService.ObtainProvincesAsync();

        Assert.Multiple(() =>
        {
            Assert.That(_advancedSearchService.Provinces.Any(province => province.Name == "San José"), Is.True);
            Assert.That(_advancedSearchService.Provinces.Any(province => province.Name == "Alajuela"), Is.True);
        });
    }

    /// <summary>
    ///     Tests that all cantons in the province selected are shown
    ///     <author>Joseph Stuart Valverde Kong C18100</author>
    /// </summary>
    [Test]
    public async Task UpdateProvince_GetsCorrectCantons()
    {
        var province = "San José";

        await _advancedSearchService.ObtainCantonsAsync(province);

        Assert.Multiple(() =>
        {
            Assert.That(_advancedSearchService.Cantons.Any(canton => canton.Name == "San José"), Is.True);
            Assert.That(_advancedSearchService.Cantons.Any(canton => canton.Name == "Tibás"), Is.True);
            Assert.That(_advancedSearchService.Cantons.Any(canton => canton.Name == "Desamparados"), Is.True);
        });
    }

    /// <summary>
    ///     Tests that all categories in the database are shown
    ///     <author>Joseph Stuart Valverde Kong C18100</author>
    /// </summary>
    [Test]
    public async Task GetCategories_ProvidesAllCategories()
    {
        await _advancedSearchService.ObtainCategoriesAsync();

        Assert.That(_advancedSearchService.Categories.Count, Is.GreaterThan(0));

        Assert.Multiple(() =>
        {
            Assert.That(_advancedSearchService.Categories.Any(category => category.Name == "Sombreros"), Is.True);
            Assert.That(_advancedSearchService.Categories.Any(category => category.Name == "Zapatos"), Is.True);
            Assert.That(_advancedSearchService.Categories.Any(category => category.Name == "Ropa"), Is.True);
            Assert.That(_advancedSearchService.Categories.Any(category => category.Name == "Accesorios"), Is.True);
        });
    }
}