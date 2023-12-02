using Locompro.Models.Entities;
using Locompro.Pages.Account;
using Locompro.Services.Auth;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Locompro.Tests.Pages.Account
{
    /// <summary>
    ///     This class contains unit tests for the ContributionsPageModel class.
    /// </summary>
    [TestFixture]
    public class ContributionsPageModelTest
    {
        [SetUp]
        public void SetUp()
        {
            _userManagerServiceMock = new Mock<IUserManagerService>();
            _configurationMock = new Mock<IConfiguration>();
            _contributionsPageModel =
                new ContributionsPageModel(_userManagerServiceMock.Object, _configurationMock.Object);
        }

        private Mock<IUserManagerService> _userManagerServiceMock;
        private Mock<IConfiguration> _configurationMock;
        private ContributionsPageModel _contributionsPageModel;

        /// <summary>
        /// Verifies that OnGetAsync correctly sets up the requested user for a valid user ID.
        ///     <author> Gabriel Molina Bulgarelli C14826 - Sprint 3 </author>
        /// </summary>
        [Test]
        public async Task OnGetAsync_ValidUserId_SetsRequestedUser()
        {
            var userId = "newUser";
            var userModel = CreateFakeUserDefault();
            _userManagerServiceMock.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync(userModel);

            await _contributionsPageModel.OnGetAsync(userId);

            Assert.That(_contributionsPageModel.RequestedUserId, Is.EqualTo(userId),
                $"Expected user ID: {userId}, Actual user ID: {_contributionsPageModel.RequestedUserId}");
            Assert.That(_contributionsPageModel.RequestedUser, Is.Not.Null, "Requested user should not be null");
            Assert.That(_contributionsPageModel.RequestedUser.Profile.Username, Is.EqualTo("temp"),
                "Unexpected user name");
        }

        /// <summary>
        /// Verifies that OnGetAsync does not set up a requested user for an invalid user ID.
        ///     <author> Gabriel Molina Bulgarelli C14826 - Sprint 3 </author>
        /// </summary>
        [Test]
        public async Task OnGetAsync_InvalidUserId_DoesNotSetRequestedUser()
        {
            var invalidUserId = "invalidUserId";
            _userManagerServiceMock.Setup(x => x.FindByIdAsync(invalidUserId)).ReturnsAsync(new User());

            await _contributionsPageModel.OnGetAsync(invalidUserId);

            Assert.That(invalidUserId, Is.EqualTo(_contributionsPageModel.RequestedUserId));
            Assert.That(_contributionsPageModel.RequestedUser, Is.Null,
                "Requested user should be null, since it doesn't exist");
            Assert.That(_contributionsPageModel.ContributionsToShow, Is.Null,
                "ContributionsToShow should be null for missing user ID");
        }

        /// <summary>
        /// Verifies that OnGetAsync does not set up contributions on a requested user without contributions.
        ///     <author> Gabriel Molina Bulgarelli C14826 - Sprint 3 </author>
        /// </summary>
        [Test]
        public async Task OnGetAsync_NoContributions_SetsContributionsToShow()
        {
            var userId = "newUser";
            var userModel = CreateFakeUserDefault();
            _userManagerServiceMock.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync(userModel);

            await _contributionsPageModel.OnGetAsync(userId);

            Assert.That(_contributionsPageModel.ContributionsToShow, Is.EqualTo("[]"),
                "ContributionsToShow should not be null");
            Assert.That(_contributionsPageModel.RequestedUser.Contributions, Is.Empty,
                "RequestedUser Contributions should not be empty but not null");
        }

        /// <summary>
        /// Verifies that OnGetAsync correctly sets up the requested user for a valid user ID and contributions.
        ///     <author> Gabriel Molina Bulgarelli C14826 - Sprint 3 </author>
        /// </summary>
        [Test]
        public async Task OnGetAsync_ValidUserId_CorrectContributions()
        {
            var userId = "newUser";
            var userModel = CreateFakeUserWithContributions();
            _userManagerServiceMock.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync(userModel);

            await _contributionsPageModel.OnGetAsync(userId);

            Assert.That(_contributionsPageModel.RequestedUserId, Is.EqualTo(userId),
                $"Expected user ID: {userId}, Actual user ID: {_contributionsPageModel.RequestedUserId}");
            Assert.That(_contributionsPageModel.RequestedUser, Is.Not.Null, "Requested user should not be null");
            Assert.That(_contributionsPageModel.ContributionsToShow, Is.Not.Null.And.Not.Empty,
                "ContributionsToShow should not be null or empty");
            Assert.That(_contributionsPageModel.RequestedUser.Contributions.Count, Is.EqualTo(2),
                "Contributions should only have the ones done by the user");
        }

        /// <summary>
        /// Verifies that OnGetAsync correctly sets up the profile information for a valid user ID.
        ///     <author> Gabriel Molina Bulgarelli C14826 - Sprint 3 </author>
        /// </summary>
        [Test]
        public async Task OnGetAsync_ValidUserId_SetsProfileInformation()
        {
            var userId = "newUser";
            var userModel = CreateFakeUserWithContributions();
            _userManagerServiceMock.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync(userModel);

            await _contributionsPageModel.OnGetAsync(userId);

            Assert.That(_contributionsPageModel.RequestedUserId, Is.EqualTo(userId),
                $"Expected user ID: {userId}, Actual user ID: {_contributionsPageModel.RequestedUserId}");
            Assert.That(_contributionsPageModel.RequestedUser, Is.Not.Null, "Requested user should not be null");
            Assert.That(_contributionsPageModel.RequestedUser.Profile, Is.Not.Null, "Profile should not be null");
            Assert.That(_contributionsPageModel.RequestedUser.Profile.Username, Is.EqualTo("temp"),
                "Unexpected user name");
            Assert.That(_contributionsPageModel.RequestedUser.Profile.Name, Is.EqualTo("N/A"), "Unexpected user name");
            Assert.That(_contributionsPageModel.RequestedUser.Profile.Address, Is.EqualTo("No fue proveído"),
                "Unexpected adress name, it should be empty since it wasn't defined");
            Assert.That(_contributionsPageModel.RequestedUser.Profile.Rating, Is.EqualTo(0), "Unexpected rating");
            Assert.That(_contributionsPageModel.RequestedUser.Profile.ContributionsCount, Is.EqualTo(2),
                "Unexpected ContributionsCount, user given has two");
            Assert.That(_contributionsPageModel.RequestedUser.Profile.Email, Is.EqualTo("anEmail@email.com"),
                "Unexpected email name");
        }

        /// <summary>
        /// Creates a default fake user for testing purposes.
        /// </summary>
        /// <returns> A fake user instance. </returns>
        private User CreateFakeUserDefault()
        {
            var user = new User
            {
                Id = "newUser",
                UserName = "temp",
                Email = "anEmail@email.com",
                Rating = 0,
                CreatedSubmissions = new List<Submission>()
            };
            return user;
        }

        /// <summary>
        /// Creates a fake user with sample contributions for testing purposes.
        /// </summary>
        /// <returns>A fake user instance with contributions.</returns>
        private User CreateFakeUserWithContributions()
        {
            var country = new Country { Name = "Country" };
            var province1 = new Province { Name = "Province1", CountryName = "Country", Country = country };
            var canton1 = new Canton { Name = "Canton1", ProvinceName = "Province1", Province = province1 };
            var category1 = new Category { Name = "Category1" };
            var category2 = new Category { Name = "Category2" };
            var category3 = new Category { Name = "Category3" };

            var user = CreateFakeUserDefault();
            user.CreatedSubmissions = new List<Submission>
            {
                new()
                {
                    UserId = "newUser",
                    EntryTime = new DateTime(2023, 10, 6, 12, 0, 0, DateTimeKind.Utc),
                    Price = 100,
                    Rating = 4,
                    Description = "Description for Submission 1",
                    StoreName = "Store1",
                    ProductId = 1,
                    User = user,
                    Store = new()
                    {
                        Name = "Store1",
                        Canton = canton1,
                        Address = "Address1",
                        Telephone = "Telephone1"
                    },
                    Product = new()
                    {
                        Id = 1,
                        Name = "Product1",
                        Model = "Model1",
                        Brand = "Brand1",
                        Categories = new List<Category> { category1, category2, category3 }
                    }
                },
                new()
                {
                    UserId = "newUser",
                    EntryTime = new DateTime(2023, 10, 6, 12, 0, 0, DateTimeKind.Utc),
                    Price = 100,
                    Rating = 4,
                    Description = "Description for Submission 1",
                    StoreName = "Store1",
                    ProductId = 1,
                    User = user,
                    Store = new()
                    {
                        Name = "Store1",
                        Canton = canton1,
                        Address = "Address1",
                        Telephone = "Telephone1"
                    },
                    Product = new()
                    {
                        Id = 1,
                        Name = "Product1",
                        Model = "Model1",
                        Brand = "Brand1",
                        Categories = new List<Category> { category2, category3 }
                    }
                }
            };

            return user;
        }
    }
}