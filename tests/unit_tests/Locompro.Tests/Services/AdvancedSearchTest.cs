using Locompro.Data;
using Locompro.Models;
using Locompro.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Locompro.Services;
using Locompro.Services.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Locompro.Tests.Services
{
    [TestFixture]
    public class AdvancedSearchTest
    {
        private ILoggerFactory _loggerFactory;
        private LocomproContext _context;
        private INamedEntityDomainService<Country, string> _countryService;
        private INamedEntityDomainService<Category, string> _categoryService;
        private AdvancedSearchInputService _advancedSearchService;

        [SetUp]
        public void SetUp()
        {
            _loggerFactory = LoggerFactory.Create(builder => { });

            var options = new DbContextOptionsBuilder<LocomproContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDbForTesting")
                .Options;

            _context = new LocomproContext(options);
            _context.Database.EnsureDeleted(); // Make sure the db is clean
            _context.Database.EnsureCreated();
            
            Country costaRica = new Country { Name = "Costa Rica" };

            _context.Set<Country>().Add(costaRica);

            Province sanJose = new Province { CountryName = "Costa Rica", Name = "San José", Country = costaRica };
            Province alajuela = new Province { CountryName = "Costa Rica", Name = "Alajuela", Country = costaRica };

            _context.Set<Province>().Add(sanJose);
            _context.Set<Province>().Add(alajuela);

            Canton sanJoseCanton = new Canton { ProvinceName = "San José", Name = "San José", Province = sanJose };
            Canton tibasCanton = new Canton { ProvinceName = "San José", Name = "Tibás", Province = sanJose };
            Canton desamparadosCanton = new Canton { ProvinceName = "San José", Name = "Desamparados", Province = sanJose };
            Canton alajuelaCanton = new Canton { ProvinceName = "Alajuela", Name = "Alajuela", Province = alajuela };
            Canton sanRamonCanton = new Canton { ProvinceName = "Alajuela", Name = "San Ramón", Province = alajuela };

            _context.Set<Canton>().Add(sanJoseCanton);
            _context.Set<Canton>().Add(tibasCanton);
            _context.Set<Canton>().Add(desamparadosCanton);
            _context.Set<Canton>().Add(alajuelaCanton);
            _context.Set<Canton>().Add(sanRamonCanton);

            new CrudRepository<User, string>(_context, _loggerFactory);

            ICrudRepository<Country, string> countryRepostory = new CrudRepository<Country, string>(_context, _loggerFactory);
            ICrudRepository<Category, string> categoryRepository = new CrudRepository<Category, string>(_context, _loggerFactory);
            
            UnitOfWork unitOfWork = new UnitOfWork(null, _loggerFactory, _context);
            unitOfWork.RegisterRepository(countryRepostory);
            unitOfWork.RegisterRepository(categoryRepository);

            this._categoryService = new NamedEntityDomainService<Category, string>(unitOfWork, _loggerFactory);
            this._countryService = new NamedEntityDomainService<Country, string>(unitOfWork, _loggerFactory);

            this._advancedSearchService = new AdvancedSearchInputService(_countryService, _categoryService);
            
            Category category1 = new Category { Name = "Sombreros" };
            Category category2 = new Category { Name = "Zapatos" };
            Category category3 = new Category { Name = "Ropa" };
            Category category4 = new Category { Name = "Accesorios" };

            _context.Set<Category>().Add(category1);
            _context.Set<Category>().Add(category2);
            _context.Set<Category>().Add(category3);
            _context.Set<Category>().Add(category4);
            
            _context.SaveChanges();
        }

        /// <summary>
        /// Tests that all provinces in the database are provided
        /// <author>Joseph Stuart Valverde Kong C18100</author>
        /// </summary>
        [Test]
        public async Task GetProvinces_ProvidesProvinces()
        {
            await this._advancedSearchService.ObtainProvincesAsync();

            Assert.Multiple(()=>
            {
                Assert.IsTrue(this._advancedSearchService.Provinces.Any(province=>province.Name == "San José"));
                Assert.IsTrue(this._advancedSearchService.Provinces.Any(province => province.Name == "Alajuela"));
            });
        }

        /// <summary>
        /// Tests that all cantons in the province selected are shown
        /// <author>Joseph Stuart Valverde Kong C18100</author>
        /// </summary>
        [Test]
        public async Task UpdateProvince_GetsCorrectCantons()
        {
            string province = "San José";
            
            await this._advancedSearchService.ObtainCantonsAsync(province);

            Assert.Multiple(() =>
            {
                Assert.IsTrue(this._advancedSearchService.Cantons.Any(canton => canton.Name == "San José"));
                Assert.IsTrue(this._advancedSearchService.Cantons.Any(canton => canton.Name == "Tibás"));
                Assert.IsTrue(this._advancedSearchService.Cantons.Any(canton => canton.Name == "Desamparados"));
            });
        }
        
        /// <summary>
        /// Tests that all categories in the database are shown
        /// <author>Joseph Stuart Valverde Kong C18100</author>
        /// </summary>
        [Test]
        public async Task GetCategories_ProvidesAllCategories()
        {
            await this._advancedSearchService.ObtainCategoriesAsync();
            
            Assert.Greater(this._advancedSearchService.Categories.Count, 0);
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(this._advancedSearchService.Categories.Any(category => category.Name == "Sombreros"));
                Assert.IsTrue(this._advancedSearchService.Categories.Any(category => category.Name == "Zapatos"));
                Assert.IsTrue(this._advancedSearchService.Categories.Any(category => category.Name == "Ropa"));
                Assert.IsTrue(this._advancedSearchService.Categories.Any(category => category.Name == "Accesorios"));
            });
        }
    }
}
