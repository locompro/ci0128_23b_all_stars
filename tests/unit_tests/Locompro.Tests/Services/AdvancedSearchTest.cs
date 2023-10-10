using Locompro.Data;
using Locompro.Models;
using Locompro.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Locompro.Services;
using Locompro.Services.Domain;

namespace Locompro.Tests.Services
{
    [TestFixture]
    public class AdvancedSearchTest
    {
        private ILoggerFactory _loggerFactory;
        private LocomproContext _context;
        private CountryService _countryService;
        private CategoryService _categoryService;
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
            
            Country CostaRica = new Country { Name = "Costa Rica" };

            _context.Set<Country>().Add(CostaRica);

            Province SanJose = new Province { CountryName = "Costa Rica", Name = "San José", Country = CostaRica };
            Province Alajuela = new Province { CountryName = "Costa Rica", Name = "Alajuela", Country = CostaRica };

            _context.Set<Province>().Add(SanJose);
            _context.Set<Province>().Add(Alajuela);

            Canton SanJoseCanton = new Canton { ProvinceName = "San José", Name = "San José", Province = SanJose };
            Canton TibasCanton = new Canton { ProvinceName = "San José", Name = "Tibás", Province = SanJose };
            Canton DesamparadosCanton = new Canton { ProvinceName = "San José", Name = "Desamparados", Province = SanJose };
            Canton AlajuelaCanton = new Canton { ProvinceName = "Alajuela", Name = "Alajuela", Province = Alajuela };
            Canton SanRamonCanton = new Canton { ProvinceName = "Alajuela", Name = "San Ramón", Province = Alajuela };

            _context.Set<Canton>().Add(SanJoseCanton);
            _context.Set<Canton>().Add(TibasCanton);
            _context.Set<Canton>().Add(DesamparadosCanton);
            _context.Set<Canton>().Add(AlajuelaCanton);
            _context.Set<Canton>().Add(SanRamonCanton);

            new UserRepository(_context, _loggerFactory);

            CountryRepository countryRepostory = new CountryRepository(_context, _loggerFactory);
            CategoryRepository categoryRepository = new CategoryRepository(_context, _loggerFactory);

            ILogger<UnitOfWork> unitOfWorkLogger = _loggerFactory.CreateLogger<UnitOfWork>();
            
            UnitOfWork unitOfWork = new UnitOfWork(unitOfWorkLogger, _context);

            this._categoryService = new CategoryService(unitOfWork, categoryRepository, _loggerFactory);
            this._countryService = new CountryService(unitOfWork, countryRepostory, _loggerFactory);

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
                Assert.IsTrue(this._advancedSearchService.Provinces.Any(Province=>Province.Name == "San José"));
                Assert.IsTrue(this._advancedSearchService.Provinces.Any(Province => Province.Name == "Alajuela"));
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
                Assert.IsTrue(this._advancedSearchService.Cantons.Any(Canton => Canton.Name == "San José"));
                Assert.IsTrue(this._advancedSearchService.Cantons.Any(Canton => Canton.Name == "Tibás"));
                Assert.IsTrue(this._advancedSearchService.Cantons.Any(Canton => Canton.Name == "Desamparados"));
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
                Assert.IsTrue(this._advancedSearchService.Categories.Any(Category => Category.Name == "Sombreros"));
                Assert.IsTrue(this._advancedSearchService.Categories.Any(Category => Category.Name == "Zapatos"));
                Assert.IsTrue(this._advancedSearchService.Categories.Any(Category => Category.Name == "Ropa"));
                Assert.IsTrue(this._advancedSearchService.Categories.Any(Category => Category.Name == "Accesorios"));
            });
        }
    }
}
